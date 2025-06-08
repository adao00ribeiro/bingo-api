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
    private readonly Dictionary<string, List<WebSocket>> _channelSubscriptions = new();
    private readonly ISubscriber _subscriber;
    private readonly IConnectionMultiplexer _redis;

    public WebSocketService(ILogger<IWebSocketService> logger, IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
        _subscriber = _redis.GetSubscriber();
    }

    public void SubscribeToChannel(string channel, WebSocket webSocket)
    {
        lock (_channelSubscriptions)
        {
            if (!_channelSubscriptions.ContainsKey(channel))
            {
                _channelSubscriptions[channel] = new List<WebSocket>();
                _subscriber.Subscribe(channel, async (redisChannel, value) =>
                {
                    await BroadcastToLocalClients(channel, value!);
                });
            }
            _channelSubscriptions[channel].Add(webSocket);
        }
        _logger.LogInformation("Client subscribed to channel {Channel}", channel);
    }
    public void UnsubscribeFromChannel(WebSocket webSocket)
    {
        lock (_channelSubscriptions)
        {
            foreach (var channel in _channelSubscriptions.Keys)
            {
                if (_channelSubscriptions[channel].Contains(webSocket))
                {
                    _channelSubscriptions[channel].Remove(webSocket);
                    _logger?.LogInformation("Client unsubscribed from channel {Channel}", channel);
                }
            }
        }
    }
    private async Task BroadcastToLocalClients(string channel, string message)
    {
        List<WebSocket> subscribers;
        lock (_channelSubscriptions)
        {
            if (!_channelSubscriptions.ContainsKey(channel)) return;
            subscribers = _channelSubscriptions[channel].ToList();
        }

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(new SocketMessage("message", channel, message, "success"), options);
        var serverMsg = Encoding.UTF8.GetBytes(json);

        foreach (var subscriber in subscribers)
        {
            if (subscriber.State == WebSocketState.Open)
            {
                await subscriber.SendAsync(new ArraySegment<byte>(serverMsg), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
    public async Task SendMessageToChannel(string channel, string message)
    {
        // Publica para o Redis, que será distribuído para todas as instâncias
        await _subscriber.PublishAsync(channel, message);
    }

    public async Task SendMessageAsync(WebSocket webSocket, string message)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true // Opcional, para formatar o JSON
        };
        var json = JsonSerializer.Serialize(new SocketMessage("message", message, "sucess"), options);
        var serverMsg = Encoding.UTF8.GetBytes(json);
        await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        _logger.LogInformation("Message sent to Client: {Message}", message);
    }

    public async Task CloseAllConnections()
    {
        List<WebSocket> activeConnections;

        lock (_channelSubscriptions)
        {
            activeConnections = _channelSubscriptions.Values.SelectMany(sockets => sockets).ToList();
        }

        foreach (var webSocket in activeConnections)
        {
            if (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.CloseReceived)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server is shutting down", CancellationToken.None);
                _logger.LogInformation("WebSocket connection closed for shutdown.");
            }
        }


        lock (_channelSubscriptions)
        {
            _channelSubscriptions.Clear();
        }
    }

    public IEnumerable<WebSocket> GetActiveConnections()
    {

        lock (_channelSubscriptions)
        {
            return _channelSubscriptions.Values.SelectMany(sockets => sockets)
                                                .Where(socket => socket.State == WebSocketState.Open)
                                                .ToList();
        }
    }
    public void Dispose()
    {

        CloseAllConnections().GetAwaiter().GetResult();
        _logger.LogInformation("All WebSocket connections closed during Dispose.");
    }
}