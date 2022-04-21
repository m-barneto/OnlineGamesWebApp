using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Dapper;
using OnlineGamesAPI.Data.Models;
using OnlineGamesAPI.Data;
using OnlineGamesAPI.Utils;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using OnlineGamesAPI.Utils.Sockets;

namespace OnlineGamesAPI.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller {
        private readonly AppDatabase db;

        public UserController(AppDatabase db) {
            this.db = db;
        }

        [HttpPut]
        [Route("login")]
        public async Task<IActionResult> Login() {
            try {
                UserModel user = Helper.GetUserModelFromJson(Request.Headers["user"]);
                UserModel? u = await db.Users.FindAsync(user.Id);
                if (u != null) {
                    // Existing user
                    u.LastSigninTime = DateTime.UtcNow.Ticks;
                } else {
                    // New user
                    user.AccountCreateTime = DateTime.UtcNow.Ticks;
                    user.LastSigninTime = DateTime.UtcNow.Ticks;
                    await db.Users.AddAsync(user);
                }

                await db.SaveChangesAsync();
                return Ok();
            } catch (Exception e) {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("ws")]
        public async Task<IActionResult> HandleWebSocketRequest() {
            UserModel user = Helper.GetUserModelFromJson(Request.Headers["user"]);

            var socket = await Request.HttpContext.WebSockets.AcceptWebSocketAsync();
            UserSocket userSocket = new UserSocket(user.Id, socket);
            await GameSocketHandler.Instance.OnConnected(userSocket);

            await Receive(userSocket, async (result, buffer) => {
                if (result.MessageType == WebSocketMessageType.Text) {
                    await GameSocketHandler.Instance.ReceiveAsync(userSocket, result, buffer);
                    return;
                } else if (result.MessageType == WebSocketMessageType.Close) {
                    await GameSocketHandler.Instance.OnDisconnected(userSocket);
                    return;
                }
            });
            return new EmptyResult();
        }

        private async Task Receive(UserSocket userSocket, Action<WebSocketReceiveResult, byte[]> handleMessage) {
            var buffer = new byte[1024 * 4];

            while (userSocket.socket.State == WebSocketState.Open) {
                var result = await userSocket.socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                        cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }
    }
}
