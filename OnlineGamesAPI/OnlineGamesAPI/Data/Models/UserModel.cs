using System.ComponentModel.DataAnnotations;

namespace OnlineGamesAPI.Data.Models {
    public class UserModel {
        [Key]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public long LastSigninTime { get; set; }

        [Required]
        public long AccountCreateTime { get; set; }

        override public string ToString() {
            return $"{Id} : {Email}\n{AccountCreateTime} - {LastSigninTime}";
        }
    }
}
