using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Structs;
using StackExchange.Redis;

namespace bingo_api.src.Services;

public class WebSocketService : IWebSocketService, IDisposable
{
    private readonly ILogger<IWebSocketService> _logger;
    private readonly IConnectionMultiplexer _redis;
    private readonly ISubscriber _subscriber;
    private Timer? _heartbeatTimer;

    // Cada usuário possui um único WebSocket e várias salas
    private readonly ConcurrentDictionary<string, WebSocket> _userConnections = new();
    private readonly ConcurrentDictionary<string, HashSet<string>> _userChannels = new();

    // Evita re-subscrever o mesmo canal no Redis
    private readonly ConcurrentDictionary<string, bool> _redisSubscribed = new();

    public WebSocketService(ILogger<IWebSocketService> logger, IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
        _subscriber = redis.GetSubscriber();
        
        // Inicia o heartbeat timer (ping a cada 30 segundos)
        _heartbeatTimer = new Timer(SendHeartbeat, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
    }

    // --- Registro de conexão (um WebSocket por usuário)
    public async Task RegisterConnectionAsync(string userId, WebSocket webSocket)
    {
        if (_userConnections.TryGetValue(userId, out var existing))
        {
            if (existing.State == WebSocketState.Open)
                await existing.CloseAsync(WebSocketCloseStatus.NormalClosure, "Nova conexão aberta", CancellationToken.None);

            _userConnections.TryRemove(userId, out _);
        }

        _userConnections[userId] = webSocket;
        _userChannels[userId] = new HashSet<string>();

        _logger.LogInformation("Usuário {UserId} conectado via WebSocket.", userId);
    }

    // --- Inscrição em um canal
    public async Task SubscribeToChannelAsync(string userId, string channel)
    {
        if (!_userConnections.ContainsKey(userId))
        {
            _logger.LogWarning("Tentativa de inscrição em canal {Channel} sem conexão ativa ({UserId}).", channel, userId);
            return;
        }

        _userChannels[userId].Add(channel);

        if (_redisSubscribed.TryAdd(channel, true))
        {
            await _subscriber.SubscribeAsync(channel, async (ch, value) =>
            {
                await BroadcastToLocalSubscribers(channel, value!);
            });
        }

        _logger.LogInformation("Usuário {UserId} inscrito no canal {Channel}.", userId, channel);
    }

    // --- Cancelar inscrição
    public Task UnsubscribeFromChannelAsync(string userId, string channel)
    {
        if (_userChannels.TryGetValue(userId, out var channels))
        {
            channels.Remove(channel);
            _logger.LogInformation("Usuário {UserId} removido do canal {Channel}.", userId, channel);
        }

        return Task.CompletedTask;
    }

    // --- Envio para um canal (broadcast via Redis)
    public async Task SendMessageToChannelAsync(string channel, string message)
    {
        await _subscriber.PublishAsync(channel, message);
        _logger.LogInformation("Mensagem publicada no canal {Channel}: {Message}", channel, message);
    }

    // --- Broadcast local (para usuários desta instância)
    private async Task BroadcastToLocalSubscribers(string channel, string message)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
        var json = JsonSerializer.Serialize(new SocketMessage("message", channel, message, "success"), options);
        var bytes = Encoding.UTF8.GetBytes(json);

        var targets = _userChannels
            .Where(u => u.Value.Contains(channel))
            .Select(u => u.Key);

        foreach (var userId in targets)
        {
            if (_userConnections.TryGetValue(userId, out var socket) && socket.State == WebSocketState.Open)
            {
                try
                {
                    await socket.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (WebSocketException ex)
                {
                    _logger.LogWarning(ex, "Erro ao enviar mensagem para usuário {UserId}", userId);
                }
            }
        }
    }

    // --- Enviar mensagem direta ao usuário
    public async Task SendToUserAsync(string userId, string message)
    {
        if (_userConnections.TryGetValue(userId, out var socket) && socket.State == WebSocketState.Open)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(new SocketMessage("message", message, "success"), options);
            var buffer = Encoding.UTF8.GetBytes(json);
            
            try
            {
                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (WebSocketException ex)
            {
                _logger.LogWarning(ex, "Erro ao enviar mensagem direta para usuário {UserId}", userId);
            }
        }
    }

    // --- Heartbeat (Ping periódico)
    private async void SendHeartbeat(object? state)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var text = JsonSerializer.Serialize(new TimerEvent());

        var json = JsonSerializer.Serialize(new SocketMessage("ping",text , "success"),options);
        var heartbeatMessage = Encoding.UTF8.GetBytes(json);

        var disconnectedUsers = new List<string>();

        foreach (var (userId, socket) in _userConnections)
        {
            if (socket.State == WebSocketState.Open)
            {
                try
                {
                    await socket.SendAsync(
                        new ArraySegment<byte>(heartbeatMessage),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
                }
                catch (WebSocketException)
                {
                    _logger.LogWarning("Cliente {UserId} desconectado durante heartbeat", userId);
                    disconnectedUsers.Add(userId);
                }
            }
            else
            {
                disconnectedUsers.Add(userId);
            }
        }

        // Limpa conexões mortas
        foreach (var userId in disconnectedUsers)
        {
            await CloseConnectionAsync(userId);
        }
    }

    // --- Fechar conexão específica
    public async Task CloseConnectionAsync(string userId)
    {
        if (_userConnections.TryRemove(userId, out var socket))
        {
            if (socket.State == WebSocketState.Open)
            {
                try
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Conexão encerrada", CancellationToken.None);
                }
                catch (WebSocketException ex)
                {
                    _logger.LogWarning(ex, "Erro ao fechar conexão do usuário {UserId}", userId);
                }
            }
        }

        _userChannels.TryRemove(userId, out _);
        _logger.LogInformation("Conexão encerrada para o usuário {UserId}.", userId);
    }

    // --- Encerrar todas as conexões
    public async Task CloseAllConnectionsAsync()
    {
        var sockets = _userConnections.Values.ToList();
        foreach (var socket in sockets)
        {
            if (socket.State == WebSocketState.Open)
            {
                try
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Servidor finalizando", CancellationToken.None);
                }
                catch (WebSocketException ex)
                {
                    _logger.LogWarning(ex, "Erro ao fechar conexão durante shutdown");
                }
            }
        }

        _userConnections.Clear();
        _userChannels.Clear();
        _logger.LogInformation("Todas as conexões WebSocket foram encerradas.");
    }

    // --- Dispose
    public void Dispose()
    {
        _heartbeatTimer?.Dispose();
        CloseAllConnectionsAsync().GetAwaiter().GetResult();
        _logger.LogInformation("WebSocketService Dispose: conexões encerradas.");
    }
}