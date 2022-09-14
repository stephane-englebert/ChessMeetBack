using System.ComponentModel.DataAnnotations;

namespace ApiChessMeet.DTO
{
    public class TournamentFormDTO
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(255)]
        public string Place { get; set; } = string.Empty;
        
        [Required]
        [Range(2,32)]
        public int PlayersMin { get; set; }

        [Required]
        [Range(2,32)]
        public int PlayersMax { get; set; }
        
        [Required]
        [Range(0,3000)]
        public int EloMin { get; set; }

        [Required]
        [Range(0,3000)]
        public int EloMax { get; set; }
        
        [Required]
        public string Categories { get; set; } = String.Empty;

        [Required]
        public bool WomenOnly { get; set; }

        [Required]
        public DateTime EndRegistration { get; set; }
    }
}
