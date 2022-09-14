using ApiChessMeet.Controllers;
using ApiChessMeet.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DalChessMeet.Interfaces;
using ApiChessMeet.Mappers;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace ApiChessMeet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _memberRepository;

        public MembersController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [Authorize(Roles ="admin,player")]
        [HttpGet]
        public IActionResult Get([FromQuery]int id)
        {
            return Ok(_memberRepository.GetMembers().Select(m => new MemberDTO(m)));
            //return Ok(_memberRepository.GetMemberById(id));
        }

        [Authorize(Roles ="admin,player")]
        [HttpPost]
        public IActionResult Post(MemberFormDTO dto)
        {
            DalChessMeet.Entities.Member entity = dto.FormToDalMembers();
            entity.Role = "player";
            //===========
            // Password
            //===========
                char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
                byte[] data = new byte[10];
                StringBuilder result = new StringBuilder(10);
                foreach (byte b in data){ result.Append(chars[b % (chars.Length)]);}
                string pwd = result.ToString();
                byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: pwd!,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8)
                );
                entity.Password = hashed;
            //========
            if (_memberRepository.ExistPseudo(entity.Pseudo))
            {
                return BadRequest("Ce pseudo est déjà utilisé par un autre membre!");
            }
            if (_memberRepository.ExistEmail(entity.Email))
            {
                return BadRequest("Cet email est déjà utilisé par un autre membre!");
            }
            if (entity.Birthdate >= DateTime.Now)
            {
                return BadRequest("Date de naissance invalide!");
            }
            _memberRepository.AddMember(entity);
            return NoContent();
        }
    }
}
