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

        [HttpPost]
        [Route("creategame/{size}")]
        public async Task<IActionResult> CreateGame(int size) {
            Console.WriteLine("Create game");
            JObject jObject = JsonConvert.DeserializeObject<JObject>(Request.Headers["user"]);


            FillerGameModel game = new FillerGameModel();
            game.GameId = "1";
            game.CreatorId = jObject["Uid"].ToString();
            game.Players = jObject["Uid"].ToString() + ',';
            game.GameCreationTime = DateTime.Now.Ticks;
            game.LastActiveTime = DateTime.Now.Ticks;
            game.GameData = Helper.InitializeFillerGameData(size);

            await db.FillerGames.AddAsync(game);
            await db.SaveChangesAsync();
            Console.WriteLine("Added game to db");
            return new EmptyResult();
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

            await HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(game.GameData));

            return new EmptyResult();
        }

        [HttpPost]
        [Route("move/{gameId}/{newColor}")]
        public async Task<IActionResult> ExecuteMove(int gameId, int newColor) {
            FillerGameModel? game = await db.FillerGames.FindAsync(gameId.ToString());
            if (game == null) {
                return NotFound();
            }

            UserModel user = Helper.GetUserModelFromJson(HttpContext.Request.Headers["user"]);
            if (!game.Players.Contains(user.Id)) {
                return BadRequest();
            }

            FillerGameBoard? gameBoard = JsonConvert.DeserializeObject<FillerGameBoard>(game.GameData);
            if (gameBoard == null) {
                return BadRequest();
            }

            int userIndex = 0;
            int opponentIndex = gameBoard.size * gameBoard.size;
            if (game.CreatorId != user.Id) {
                // user is the second player, their square is at size, size
                userIndex = gameBoard.size * gameBoard.size;
                opponentIndex = 0;
            }

            // If userIndex or opponentIndex is the same color as newColor, bad request
            if (gameBoard.GetColor(userIndex) == newColor) {
                return BadRequest();
            } else if (gameBoard.GetColor(opponentIndex) == newColor) {
                return BadRequest();
            }

            gameBoard.ExecuteMove(userIndex, newColor);

            return Ok();
        }
    }
}
