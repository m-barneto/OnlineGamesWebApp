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
    }
}
