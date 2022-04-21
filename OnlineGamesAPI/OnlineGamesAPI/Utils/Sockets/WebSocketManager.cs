using OnlineGamesAPI.Data.Models;
using OnlineGamesAPI.Utils.Sockets;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace OnlineGamesAPI.Utils {
    public class WebSocketManager {
        public static ConcurrentDictionary<string, UserSocket> clients = new();

        public static UserSocket GetSocketById(string id) {
            return clients.FirstOrDefault(p => p.Key == id).Value;
        }

        public static ConcurrentDictionary<string, UserSocket> GetAll() {
            return clients;
        }

        public static void AddSocket(UserSocket userSocket) {
            clients.TryAdd(userSocket.userId, userSocket);
        }

        public static async Task RemoveSocket(string id) {
            UserSocket userSocket;

            clients.TryRemove(id, out userSocket!);

            await userSocket.socket!.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the SocketManager",
                                    cancellationToken: CancellationToken.None);
        }

        /*
        public static async Task HandleSocketRequest(HttpRequest req, WebSocket ws) {
            UserModel user = Helper.GetUserModelFromJson(req.Headers["user"]);
            clients.TryAdd(user.Id, ws);
            clients.TryRemove(KeyValuePair.Create(user.Id, ws));
        }
         */
    }
}
