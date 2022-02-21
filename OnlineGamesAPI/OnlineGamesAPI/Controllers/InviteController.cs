using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineGamesAPI.Data;
using OnlineGamesAPI.Data.Models;
using OnlineGamesAPI.Utils;

namespace OnlineGamesAPI.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class InviteController : Controller {
        private readonly AppDatabase db;

        public InviteController(AppDatabase db) { this.db = db; }

        [HttpGet]
        [Route("invites")]
        public async Task<IActionResult> Invites() {
            try {
                UserModel user = Helper.GetUserModelFromJson(Request.Headers["user"]);
                UserModel? u = await db.Users.FindAsync(user.Id);

                if (u == null) {
                    // Unregistered user
                    return NotFound();
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
    }
}
