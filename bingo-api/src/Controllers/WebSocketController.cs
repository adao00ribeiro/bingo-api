using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers;

[ApiController]
[Route("ws")]
public class WebSocketController : ControllerBase
{
    private readonly IWebSocketService _wsService;
    private readonly ILogger<WebSocketController> _logger;

    public WebSocketController(IWebSocketService wsService, ILogger<WebSocketController> logger)
    {
        _wsService = wsService;
        _logger = logger;
    }

    [HttpGet("{userId}")]
    public async Task Get(string userId)
    {
        if (!HttpContext.WebSockets.IsWebSocketRequest)
        {
            HttpContext.Response.StatusCode = 400;
            return;
        }

        var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
        await _wsService.RegisterConnectionAsync(userId, socket);

        var buffer = new byte[1024 * 4];

        try
        {
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _wsService.CloseConnectionAsync(userId);
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    try
                    {
                        var data = JsonSerializer.Deserialize<SocketCommand>(msg);

                        if (data is not null)
                        {
                            switch (data.command?.ToLowerInvariant())
                            {
                                case "subscribe":
                                    await _wsService.SubscribeToChannelAsync(userId, data.channel!);
                                    await _wsService.SendMessageToChannelAsync(userId, $"Subscribed to {data.channel}");
                                    break;
                                case "unsubscribe":
                                    await _wsService.UnsubscribeFromChannelAsync(userId, data.channel!);
                                    await _wsService.SendMessageToChannelAsync(userId, $"Unsubscribed from {data.channel}");
                                    break;
                                case "message":
                                    await _wsService.SendMessageToChannelAsync(data.channel!, data.message!);
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Erro processando mensagem WS do usuário {UserId}", userId);
                    }
                }
            }
        }
        catch (WebSocketException ex)
        {
            _logger.LogError(ex, "WebSocket error para usuário {UserId}", userId);
        }
        finally
        {
            await _wsService.CloseConnectionAsync(userId);
        }
    }

    private record SocketCommand(string? command, string? channel, string? message);
}