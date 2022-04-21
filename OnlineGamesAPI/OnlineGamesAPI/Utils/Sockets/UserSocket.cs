using System.Net.WebSockets;

namespace OnlineGamesAPI.Utils.Sockets {
    public class UserSocket {
        public string userId;
        public WebSocket socket;
        public UserSocket(string userId, WebSocket socket) {
            this.userId = userId;
            this.socket = socket;
        }
    }
}
