using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineGamesAPI.Data;
using OnlineGamesAPI.Data.Models;
using OnlineGamesAPI.Utils;

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
            game.GameId = "0";
            game.CreatorId = jObject["id"].ToString();
            game.Players = jObject["id"].ToString() + ',';
            game.GameCreationTime = DateTime.Now.Ticks;
            game.LastActiveTime = DateTime.Now.Ticks;
            game.GameData = Helper.InitializeFillerGameData(size);

            await db.FillerGames.AddAsync(game);
            Console.WriteLine("Added game to db");
            return Ok();
        }

        [HttpGet]
        [Route("getgamestate")]
        public async Task<IActionResult> GetGameState(int gameId) {





            return Ok();
        }
    }
}
