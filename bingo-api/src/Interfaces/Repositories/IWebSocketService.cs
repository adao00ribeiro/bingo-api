using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace bingo_api.src.Interfaces.Repositories;

public interface IWebSocketService
{
        Task RegisterConnectionAsync(string userId, WebSocket socket);
        Task SubscribeToChannelAsync(string userId, string channel);
        Task UnsubscribeFromChannelAsync(string userId, string channel);
        Task SendMessageToChannelAsync(string channel, string message);
        Task CloseConnectionAsync(string userId);
}
