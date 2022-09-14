using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalChessMeet.Entities
{
    public class Registration
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string TournamentGuid { get; set; } = string.Empty;
    }
}
