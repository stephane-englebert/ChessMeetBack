using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalChessMeet.Entities
{
    public class MemberDetails
    {
        public int Id { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Pseudo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; } = string.Empty;
        public int Elo { get; set; }

        //public MemberDetails(int id, string role, string pseudo, string email, DateTime birthdate, string gender, int elo)
        //{
        //    Id = id;
        //    Role = role;
        //    Pseudo = pseudo;
        //    Email = email;
        //    Birthdate = birthdate;
        //    Gender = gender;
        //    Elo = elo;
        //}
    }
}
