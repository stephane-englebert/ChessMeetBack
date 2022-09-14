using System.ComponentModel.DataAnnotations;

namespace ApiChessMeet.DTO
{
    public class MemberLoginFormDTO
    {
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
