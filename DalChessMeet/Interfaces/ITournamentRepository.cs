using DalChessMeet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalChessMeet.Interfaces
{
    public interface ITournamentRepository
    {
        IEnumerable<Entities.Tournament> GetTournaments();
        IEnumerable<Entities.MemberDetails> GetTournamentPlayersDetails(string g);
        void AddTournament(Entities.Tournament tournament);
        bool ExistGuid(Guid g);
        void DeleteTournament(Guid id);
        Entities.TournamentDetails GetTournamentByGuid(string g);
    }
}
