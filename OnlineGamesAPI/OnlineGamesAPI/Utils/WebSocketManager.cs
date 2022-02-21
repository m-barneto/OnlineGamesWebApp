using OnlineGamesAPI.Data.Models;
using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace OnlineGamesAPI.Utils {
    public class WebSocketManager {
        public static ConcurrentDictionary<string, WebSocket> clients = new();

        public static WebSocket GetSocketById(string id) {
            return clients.FirstOrDefault(p => p.Key == id).Value;
        }

        public static ConcurrentDictionary<string, WebSocket> GetAll() {
            return clients;
        }

        public static string GetId(WebSocket socket) {
            return clients.FirstOrDefault(p => p.Value == socket).Key;
        }
        public static void AddSocket(string id, WebSocket socket) {
            clients.TryAdd(id, socket);
        }

        public static async Task RemoveSocket(string id) {
            WebSocket socket;
            clients.TryRemove(id, out socket!);

            await socket!.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the SocketManager",
                                    cancellationToken: CancellationToken.None);
        }


        public static async Task HandleSocketRequest(HttpRequest req, WebSocket ws) {
            UserModel user = Helper.GetUserModelFromJson(req.Headers["user"]);
            clients.TryAdd(user.Id, ws);
            while (ws.State == WebSocketState.Open) {

            }
            clients.TryRemove(KeyValuePair.Create(user.Id, ws));
        }
    }
}
