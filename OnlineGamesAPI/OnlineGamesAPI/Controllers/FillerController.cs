using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineGamesAPI.Data;
using OnlineGamesAPI.Data.Models;
using OnlineGamesAPI.Utils;
using System.Text;

namespace OnlineGamesAPI.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class FillerController : Controller {
        private readonly AppDatabase db;

        public FillerController(AppDatabase db) {
            this.db = db;
        }

        [HttpGet]
        [Route("getgamestate/{gameId}")]
        public async Task<IActionResult> GetGameState(string gameId) {
            FillerGameModel? game = await db.FillerGames.FindAsync(gameId);
            if (game == null) {
                return NotFound();
            }

            UserModel user = Helper.GetUserModelFromJson(HttpContext.Request.Headers["user"]);
            if (!game.Players.Contains(user.Id)) {
                return BadRequest();
            }
            Console.WriteLine(game.GameData);
            await HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(game.GameData));

            return new EmptyResult();
        }

        [HttpPost]
        [Route("move/{gameId}/{newColor}")]
        public async Task<IActionResult> ExecuteMove(string gameId, int newColor) {

            FillerGameModel? game = await db.FillerGames.FindAsync(gameId);
            if (game == null) {
                return NotFound();
            }

            UserModel user = Helper.GetUserModelFromJson(HttpContext.Request.Headers["user"]);
            if (!game.Players.Contains(user.Id)) {
                return BadRequest();
            }

            if (user.Id != game.TurnId) {
                return BadRequest();
            }

            FillerGameBoard? gameBoard = JsonConvert.DeserializeObject<FillerGameBoard>(game.GameData);
            if (gameBoard == null) {
                return StatusCode(500);
            }

            int userIndex = 0;
            int opponentIndex = gameBoard.size * gameBoard.size - 1;
            if (game.CreatorId != user.Id) {
                // user is the second player, their square is at size, size
                userIndex = gameBoard.size * gameBoard.size - 1;
                opponentIndex = 0;
            }
            // If userIndex or opponentIndex is the same color as newColor, bad request
            if (gameBoard.GetColorByIndex(userIndex) == newColor) {
                return BadRequest();
            } else if (gameBoard.GetColorByIndex(opponentIndex) == newColor) {
                return BadRequest();
            }


            gameBoard.ExecuteMove(userIndex, newColor);

            string[] users = game.Players.Split(",");
            string opponentId = users[0];
            if (users[0] == user.Id) {
                // send to users[1]
                opponentId = users[1];
            }

            game.GameData = JsonConvert.SerializeObject(gameBoard);
            game.TurnId = opponentId;
            game.LastActiveTime = DateTime.UtcNow.Ticks;

            db.FillerGames.Update(game);
            await db.SaveChangesAsync();


            await GameSocketHandler.Instance.SendMessageAsync(opponentId, gameId);

            return Ok();
        }
    }
}
