namespace ApiChessMeet.DTO
{
    public class MemberDetailsDTO
    {
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Pseudo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; } = string.Empty;
        public int Elo { get; set; }

        public MemberDetailsDTO(int id, string role, string pseudo, string email, DateTime birthdate, string gender,int elo)
        {
            Id = id;
            Role = role;
            Pseudo = pseudo;
            Email = email;
            Birthdate = birthdate;
            Gender = gender;
            Elo = elo;
        }
    }
}
