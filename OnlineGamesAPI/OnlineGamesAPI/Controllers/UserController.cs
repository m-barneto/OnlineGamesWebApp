using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Dapper;
using OnlineGamesAPI.Data.Models;
using OnlineGamesAPI.Data;
using OnlineGamesAPI.Utils;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

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
                    u.LastSigninTime = DateTime.Now.Ticks;
                } else {
                    // New user
                    user.AccountCreateTime = DateTime.Now.Ticks;
                    user.LastSigninTime = DateTime.Now.Ticks;
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
            await GameSocketHandler.Instance.OnConnected(user.Id, socket);

            await Receive(socket, async (result, buffer) => {
                if (result.MessageType == WebSocketMessageType.Text) {
                    await GameSocketHandler.Instance.ReceiveAsync(socket, result, buffer);
                    return;
                } else if (result.MessageType == WebSocketMessageType.Close) {
                    await GameSocketHandler.Instance.OnDisconnected(socket);
                    return;
                }
            });
            return new EmptyResult();
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage) {
            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open) {
                var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                        cancellationToken: CancellationToken.None);

                handleMessage(result, buffer);
            }
        }
    }
}
