using ApiChessMeet.DTO;
using DalChessMeet.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiChessMeet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly IMemberRepository _memberRepository;

        public TokenController(IConfiguration configuration, IMemberRepository memberRepository)
        {
            _configuration = configuration;
            _memberRepository = memberRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post(MemberLoginFormDTO member)
        {
            if(member != null && member.UserName != null && member.Password != null)
            {
                DalChessMeet.Entities.MemberDetails dbMember = await GetMemberInfos(member.UserName, member.Password);
                if(dbMember != null)
                {
                    Claim[] claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim(ClaimTypes.Role, dbMember.Role),
                        new Claim(ClaimTypes.NameIdentifier, dbMember.Id.ToString()),
                        new Claim("Id", dbMember.Id.ToString()),
                        new Claim("Role", dbMember.Role),
                        new Claim("Pseudo", dbMember.Pseudo),
                        new Claim("Email", dbMember.Email),
                        new Claim("Birthdate", dbMember.Birthdate.ToString()),
                        new Claim("Gender", dbMember.Gender),
                        new Claim("Elo", dbMember.Elo.ToString())
                    };
                    SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    JwtSecurityToken token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires:DateTime.UtcNow.AddMinutes(30),
                        signingCredentials: signIn
                    );
                    string Token = new JwtSecurityTokenHandler().WriteToken(token);
                    MemberDetailsDTO memberDetails = new MemberDetailsDTO(
                        dbMember.Id,
                        dbMember.Role,
                        dbMember.Pseudo,
                        dbMember.Email,
                        dbMember.Birthdate,
                        dbMember.Gender,
                        dbMember.Elo
                    );
                    return Ok(new TokenDTO(Token,memberDetails));
                }
                else
                {
                    return BadRequest("Paramètres de connexion invalides!");
                }
            }
            else { 
                return BadRequest("Paramètres de connexion invalides!"); 
            }
        }

        private async Task<DalChessMeet.Entities.MemberDetails> GetMemberInfos(string userName, string pwd)
        {
            return _memberRepository.GetMemberByCredentials(userName, pwd);
        }
    }
}
