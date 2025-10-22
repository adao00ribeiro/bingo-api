using System.Net.WebSockets;
using System.Text;
using bingo_api.src.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace bingo_api.src.Controllers;

    [ApiController]
    [Route("ws")]
    public class WebSocketController : ControllerBase
    {
        private readonly IWebSocketService _wsService;

        public WebSocketController(IWebSocketService wsService)
        {
            _wsService = wsService;
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
                        var data = System.Text.Json.JsonSerializer.Deserialize<SocketCommand>(msg);

                        if (data is not null)
                        {
                            switch (data.command?.ToLowerInvariant())
                            {
                                case "subscribe":
                                    await _wsService.SubscribeToChannelAsync(userId, data.channel!);
                                    break;
                                case "unsubscribe":
                                    await _wsService.UnsubscribeFromChannelAsync(userId, data.channel!);
                                    break;
                                case "message":
                                    await _wsService.SendMessageToChannelAsync(data.channel!, data.message!);
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro processando mensagem WS: {ex.Message}");
                    }
                }
            }
        }

        private record SocketCommand(string? command, string? channel, string? message);
    }

