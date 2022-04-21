using OnlineGamesAPI.Utils.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace OnlineGamesAPI.Utils {
    public class GameSocketHandler : WebSocketHandler {
        private static readonly GameSocketHandler instance = new GameSocketHandler();
        static GameSocketHandler() { }
        private GameSocketHandler() { }
        public static GameSocketHandler Instance { get { return instance; } }


        public override async Task OnConnected(UserSocket userSocket) {
            await base.OnConnected(userSocket);
            await SendMessageToAllAsync($"{userSocket.userId} is now connected");
        }

        public override async Task ReceiveAsync(UserSocket userSocket, WebSocketReceiveResult result, byte[] buffer) {
            var message = $"{userSocket.userId} said: {Encoding.UTF8.GetString(buffer, 0, result.Count)}";
            Console.WriteLine(message);
            await SendMessageToAllAsync(message);
        }

        public override async Task OnDisconnected(UserSocket userSocket) {
            Console.WriteLine("Socket disconnected");
            await base.OnDisconnected(userSocket);
        }
    }
}
