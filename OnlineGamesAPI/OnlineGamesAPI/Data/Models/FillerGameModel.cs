using System.ComponentModel.DataAnnotations;

namespace OnlineGamesAPI.Data.Models {
    public class FillerGameModel {
        [Key]
        public string GameId { get; set; } = string.Empty;

        [Required]
        public string CreatorId { get; set; } = string.Empty;

        [Required]
        public string Players { get; set; } = string.Empty;

        [Required]
        public string TurnId { get; set; } = string.Empty;

        [Required]
        public long GameCreationTime { get; set; }

        [Required]
        public long LastActiveTime { get; set; }

        [Required]
        public string GameData { get; set; } = string.Empty;
    }
}
