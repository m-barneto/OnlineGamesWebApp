using System.Net.WebSockets;
using System.Text;

namespace OnlineGamesAPI.Utils {
    public abstract class WebSocketHandler {
        public virtual async Task OnConnected(string id, WebSocket socket) {
            WebSocketManager.AddSocket(id, socket);
        }

        public virtual async Task OnDisconnected(WebSocket socket) {
            await WebSocketManager.RemoveSocket(WebSocketManager.GetId(socket));
        }

        public async Task SendMessageAsync(WebSocket socket, string message) {
            if (socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                    offset: 0,
                                                                    count: message.Length),
                                    messageType: WebSocketMessageType.Text,
                                    endOfMessage: true,
                                    cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageAsync(string socketId, string message) {
            await SendMessageAsync(WebSocketManager.GetSocketById(socketId), message);
        }

        public async Task SendMessageToAllAsync(string message) {
            foreach (var pair in WebSocketManager.GetAll()) {
                if (pair.Value.State == WebSocketState.Open)
                    await SendMessageAsync(pair.Value, message);
            }
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }
}
