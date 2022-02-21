using System.Net.WebSockets;
using System.Text;

namespace OnlineGamesAPI.Utils {
    public class GameSocketHandler : WebSocketHandler {
        private static readonly GameSocketHandler instance = new GameSocketHandler();
        static GameSocketHandler() { }
        private GameSocketHandler() { }
        public static GameSocketHandler Instance { get { return instance; } }


        public override async Task OnConnected(string id, WebSocket socket) {
            await base.OnConnected(id, socket);
            await SendMessageToAllAsync($"{id} is now connected");
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer) {
            var socketId = WebSocketManager.GetId(socket);
            var message = $"{socketId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";

            await SendMessageToAllAsync(message);
        }
    }
}
