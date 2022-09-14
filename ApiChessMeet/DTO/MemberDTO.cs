namespace ApiChessMeet.DTO
{
    public class MemberDTO
    {
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Pseudo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; } = string.Empty;
        public int Elo { get; set; }

        public MemberDTO(DalChessMeet.Entities.Member member)
        {
            Id = member.Id;
            Role = member.Role;
            Pseudo = member.Pseudo;
            Email = member.Email;
            Birthdate = member.Birthdate;
            Gender = member.Gender;
            Elo = member.Elo;
        }
    }
}
