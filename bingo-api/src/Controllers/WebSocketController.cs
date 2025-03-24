using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using bingo_api.src.Controllers.Shared;
using bingo_api.src.Interfaces.Repositories;
using bingo_api.src.Structs;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
namespace bingo_api.src.Controllers;


[ApiVersion("1.0")]
public class WebSocketController : ApiControllerBase, IDisposable
{
    private readonly ILogger<WebSocketController> _logger;
    private readonly IWebSocketService _webSocketService;
    private const int BufferSize = 64 * 1024;
    private Timer _heartbeatTimer;
    public WebSocketController(ILogger<WebSocketController> logger, IWebSocketService webSocketService)
    {
        _logger = logger;
        _webSocketService = webSocketService;

        _heartbeatTimer = new Timer(SendHeartbeat, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
    }
    [HttpGet("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            _logger.Log(LogLevel.Information, "WebSocket connection established");
            await HandleWebSocketConnection(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task HandleWebSocketConnection(WebSocket webSocket)
    {
        var buffer = new byte[BufferSize];

        try
        {
            WebSocketReceiveResult result;
            do
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text && !result.CloseStatus.HasValue)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    _logger.LogInformation("Message received from Client: {Message}", message);

                    await ProcessMessageAsync(message, webSocket);
                }

            } while (!result.CloseStatus.HasValue);

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            _logger.LogInformation("WebSocket connection closed");
        }
        catch (WebSocketException e)
        {
            _logger.LogError(e, "WebSocket error occurred");
        }
        finally
        {
            await CloseConnectionAsync(webSocket);
        }
    }

    private async Task ProcessMessageAsync(string message, WebSocket webSocket)
    {
        try
        {
            var jsonDocument = JsonDocument.Parse(message);
            var root = jsonDocument.RootElement;

            if (root.TryGetProperty("command", out var commandProperty) &&
                root.TryGetProperty("channel", out var channelProperty))
            {
                var command = commandProperty.GetString();
                var channel = channelProperty.GetString();

                switch (command)
                {
                    case "subscribe" when channel != null:
                        _webSocketService.SubscribeToChannel(channel, webSocket);
                        await _webSocketService.SendMessageAsync(webSocket, $"Subscribed to {channel}");
                        break;

                    case "message" when root.TryGetProperty("message", out var contentProperty):
                        var channelMessage = contentProperty.GetString();
                        if (channelMessage != null)
                        {
                            await _webSocketService.SendMessageToChannel(channel, channelMessage);
                            await  _webSocketService.SendMessageAsync(webSocket, "Message sent to channel");
                        }
                        break;

                    default:
                        _logger.LogWarning("Invalid command or missing channel in message.");
                           await  _webSocketService.SendMessageAsync(webSocket, $"Invalid command: {command}");
                        break;
                }
            }
            else
            {
                _logger.LogWarning("Invalid JSON message format. Required fields 'command' and 'channel' are missing.");
                    await  _webSocketService.SendMessageAsync(webSocket, "Error parsing MessagePack data");
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error parsing JSON message.");
              await  _webSocketService.SendMessageAsync(webSocket, "Server error");
        }
    }

    private async Task CloseConnectionAsync(WebSocket webSocket)
    {
        _webSocketService.UnsubscribeFromChannel(webSocket);
        if (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.CloseReceived)
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
        }
        _logger.LogInformation("WebSocket connection closed (finally block)");
    }
    private async void SendHeartbeat(object state)
    {
        var activeConnections = _webSocketService.GetActiveConnections();
        foreach (var webSocket in activeConnections)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        WriteIndented = true // Opcional, para formatar o JSON
                    };
                    var json = JsonSerializer.Serialize(new SocketMessage("ping", "ping","success"), options);
                    var heartbeatMessage = Encoding.UTF8.GetBytes(json);
                    await webSocket.SendAsync(new ArraySegment<byte>(heartbeatMessage), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (WebSocketException)
                {
                    _logger.LogWarning("Client disconnected due to inactivity.");
                    await CloseConnectionAsync(webSocket);
                }
            }
        }
    }
  

    public void Dispose()
    {
        _heartbeatTimer?.Dispose();
        // _webSocketService.CloseAllConnections(); 
        _logger.LogInformation("Disposed WebSocketController and closed all connections.");
    }
}
