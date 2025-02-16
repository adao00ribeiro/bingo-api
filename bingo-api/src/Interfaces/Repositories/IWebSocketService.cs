using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace bingo_api.src.Interfaces.Repositories;

public interface IWebSocketService
{
    void SubscribeToChannel(string channel, WebSocket webSocket);
    Task SendMessageToChannel(string channel, string message);
    void UnsubscribeFromChannel(WebSocket webSocket);
    Task SendMessageAsync(WebSocket webSocket, string message);
    Task CloseAllConnections();
    IEnumerable<WebSocket> GetActiveConnections();
}
