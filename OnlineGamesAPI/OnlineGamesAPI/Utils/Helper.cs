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

        public static string InitializeFillerGameData(int size) {
            FillerGameBoard filler = new FillerGameBoard();
            filler.size = size;

            filler.board = new List<int>();

            for (int i = 0; i < size * size; i++) {
                filler.board.Add(Random.Shared.Next(0, 5));
            }
            Console.WriteLine(JsonConvert.SerializeObject(filler));

            return JsonConvert.SerializeObject(filler);
        }
    }
}
