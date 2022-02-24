using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace OnlineGamesAPI.Data.Models {
    public class InviteModel {
        [Key]
        public string InviteCode { get; set; } = string.Empty;

        [Required]
        public string CreatorId { get; set; } = string.Empty;

        [Required]
        public long InviteCreationTime { get; set; }

        [Required]
        public string InviteData { get; set; } = string.Empty;

        override public string ToString() {
            return $"{CreatorId}({InviteCreationTime}): {InviteData}";
        }
    }
}
