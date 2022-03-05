using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineGamesAPI.Data;
using OnlineGamesAPI.Data.Models;
using OnlineGamesAPI.Utils;
using System.Text;

namespace OnlineGamesAPI.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class InviteController : Controller {
        private readonly AppDatabase db;

        public InviteController(AppDatabase db) { this.db = db; }

        private async Task<string?> GetInviteCode() {
            string invite = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
            int timeout = 0;

            while ((await (from i in db.Invites where i.InviteCode == invite select i).ToListAsync()).Count > 0) {
                Console.WriteLine("Creating new invite since first was found in the db");
                if (timeout > 30) {
                    // unlucky ig
                    Console.WriteLine("Tried to create an invite 30 times, exiting now");
                    return null;
                }
                invite = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
                timeout++;
            }
            Console.WriteLine($"Invite code {invite} has been created.");
            return invite;
        }

        private async Task<string?> GetGameCode() {
            string code = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            int timeout = 0;

            while ((await (from g in db.FillerGames where g.GameId == code select g).ToListAsync()).Count > 0) {
                Console.WriteLine("Creating new game id since first was found in the db");
                if (timeout > 30) {
                    // unlucky ig
                    Console.WriteLine("Tried to create a game id 30 times, exiting now");
                    return null;
                }
                code = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                timeout++;
            }
            Console.WriteLine($"Game id {code} has been created.");
            return code;
        }

        [HttpGet]
        [Route("invites")]
        public async Task<IActionResult> Invites() {
            try {
                UserModel user = Helper.GetUserModelFromJson(Request.Headers["user"]);
                UserModel? u = await db.Users.FindAsync(user.Id);

                if (u == null) {
                    // Unregistered user
                    return BadRequest();
                }

                // Existing user
                Console.WriteLine("Existing user");
                List<InviteModel> invites = await (from i in db.Invites
                                                   where i.CreatorId == user.Id
                                                   select i).ToListAsync();
                return View(invites);
            } catch (Exception e) {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("createinvite/{boardSize}")]
        public async Task<IActionResult> CreateInvite(int boardSize) {
            try {
                UserModel user = Helper.GetUserModelFromJson(Request.Headers["user"]);
                UserModel? u = await db.Users.FindAsync(user.Id);

                if (u == null) {
                    // Unregistered user
                    return BadRequest();
                }

                // Existing user
                List<InviteModel> invites = await (from i in db.Invites
                                                   where i.CreatorId == user.Id
                                                   select i).ToListAsync();
                string? inviteCode = await GetInviteCode();
                if (inviteCode == null) {
                    Console.WriteLine("Failed while creating an invite.");
                    return StatusCode(500);
                }
                InviteModel invite = new InviteModel() {
                    CreatorId = u.Id,
                    InviteCode = inviteCode,
                    InviteCreationTime = DateTime.Now.Ticks,
                    InviteData = "" + boardSize
                };
                await db.Invites.AddAsync(invite);
                await db.SaveChangesAsync();

                await HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(invite)));

                return new EmptyResult();
            } catch (Exception e) {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("joininvite/{inviteCode}")]
        public async Task<IActionResult> JoinInvite(string inviteCode) {
            try {
                UserModel user = Helper.GetUserModelFromJson(Request.Headers["user"]);
                UserModel? u = await db.Users.FindAsync(user.Id);

                // Unregistered user
                if (u == null) return BadRequest();
                
                List<InviteModel> invites = (await (from i in db.Invites where i.InviteCode == inviteCode select i).ToListAsync());

                // Invite doesn't exist
                await Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes("No invite found."));
                if (invites.Count == 0) return BadRequest();

                InviteModel invite = invites[0];


                // User requesting to join is the creator
                // if (u.Id == invite.CreatorId) return BadRequest();

                // Invite is valid, create a matching game then delete it from the invites db
                string? gameId = await GetGameCode();
                if (gameId == null) return StatusCode(500);

                FillerGameModel filler = new FillerGameModel() {
                    CreatorId = invite.CreatorId,
                    Players = invite.CreatorId + ',' + u.Id,
                    GameId = gameId,
                    GameCreationTime = DateTime.Now.Ticks,
                    LastActiveTime = DateTime.Now.Ticks,
                    GameData = Helper.InitializeFillerGameData(int.Parse(invite.InviteData))
                };

                await db.FillerGames.AddAsync(filler);

                db.Invites.Remove(invite);

                await db.SaveChangesAsync();

                await HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(filler.GameId)));

                return new EmptyResult();
            } catch (Exception e) {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }
    }
}
