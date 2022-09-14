namespace ApiChessMeet.DTO
{
    public class TokenDTO
    {
        public string Token { get; set; } = string.Empty;
        public MemberDetailsDTO LoggedMember { get; set; }
        public TokenDTO(string token, MemberDetailsDTO member)
        {
            Token = token;
            LoggedMember = member;
        }
    }
}
