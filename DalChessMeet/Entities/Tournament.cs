using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalChessMeet.Entities
{
    public class Tournament
    {
        public int Id { get; set; }
        public string Guid { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        public int PlayersMin { get; set; }
        public int PlayersMax { get; set; }
        public int EloMin { get; set; }
        public int EloMax { get; set; }
        public string Categories { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int CurrentRound { get; set; }
        public bool WomenOnly { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime EndRegistration { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
