using System.ComponentModel.DataAnnotations;

namespace ApiChessMeet.DTO
{
    public class MemberFormDTO
    {
        [Required]
        [MaxLength(50)]
        public string Pseudo { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        [EnumDataType(typeof(Enums.EnumGender), ErrorMessage = "Genre invalide!")]
        public string Gender { get; set; } = string.Empty;

        [Required]
        [Range(0,3000)]
        public int Elo { get; set; }
    }
}
