using System.ComponentModel.DataAnnotations;

namespace ApiChessMeet.DTO
{
    public class RegistrationFormDTO
    {
        [Required]
        [MaxLength(255)]
        public string TournamentGuid { get; set; } = string.Empty;
    }
}
