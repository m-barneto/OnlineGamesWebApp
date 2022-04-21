using OnlineGamesAPI.Utils.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace OnlineGamesAPI.Utils {
    public abstract class WebSocketHandler {
        public virtual async Task OnConnected(UserSocket userSocket) {
            WebSocketManager.AddSocket(userSocket);
        }

        public virtual async Task OnDisconnected(UserSocket userSocket) {
            await WebSocketManager.RemoveSocket(userSocket.userId);
        }

        public async Task SendMessageAsync(UserSocket userSocket, string message) {
            if (userSocket.socket.State != WebSocketState.Open)
                return;

            await userSocket.socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                    offset: 0,
                                                                    count: message.Length),
                                    messageType: WebSocketMessageType.Text,
                                    endOfMessage: true,
                                    cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageAsync(string socketId, string message) {
            UserSocket socket = WebSocketManager.GetSocketById(socketId);
            if (socket == null) {
                return;
            }
            await SendMessageAsync(socket, message);
        }

        public async Task SendMessageToAllAsync(string message) {
            foreach (var pair in WebSocketManager.GetAll()) {
                if (pair.Value.socket.State == WebSocketState.Open)
                    await SendMessageAsync(pair.Value, message);
            }
        }

        public abstract Task ReceiveAsync(UserSocket userSocket, WebSocketReceiveResult result, byte[] buffer);
    }
}
