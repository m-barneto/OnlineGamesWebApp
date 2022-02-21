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
            return Ok();
        }

        [HttpGet]
        [Route("getgamestate/{gameId}")]
        public async Task<IActionResult> GetGameState(int gameId) {
            FillerGameModel game = await db.FillerGames.FindAsync(gameId.ToString());
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
    }
}
