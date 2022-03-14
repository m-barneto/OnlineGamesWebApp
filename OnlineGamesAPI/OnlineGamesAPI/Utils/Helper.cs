using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OnlineGamesAPI.Data.Models;
using System.Text;

namespace OnlineGamesAPI.Utils {
    public class Helper {
        public static UserModel GetUserModelFromJson(string json) {
            JObject jObject = JsonConvert.DeserializeObject<JObject>(json)!;

            if (jObject["Uid"]!.ToString().Equals("dev")) {
                return new UserModel() {
                    Id = jObject["Uid"]!.ToString(),
                    Email = "dev@test.com",
                    AccountCreateTime = 0L,
                    LastSigninTime = 0L
                };
            }

            return new UserModel() {
                Id = jObject["Uid"]!.ToString(),
                Email = jObject["Claims"]!["email"]!.ToString(),
                AccountCreateTime = 0L,
                LastSigninTime = jObject["Claims"]!["auth_time"]!.ToObject<long>()
            };

        }

        private static string InitializeFillerGameData(int size) {
            FillerGameBoard filler = new FillerGameBoard(size);
            filler.size = size;

            filler.board = new List<FillerGameBoard.Tile>();

            for (int i = 0; i < size * size; i++) {
                filler.board.Add(new FillerGameBoard.Tile(0, Random.Shared.Next(0, 5)));
            }
            while (filler.board[0].color == filler.board[size * size - 1].color) {
                filler.board[0].color = Random.Shared.Next(0, 5);
            }
            filler.board[0].team = 1;
            filler.board[size * size - 1].team = 2;

            return JsonConvert.SerializeObject(filler);
        }
    }
}
