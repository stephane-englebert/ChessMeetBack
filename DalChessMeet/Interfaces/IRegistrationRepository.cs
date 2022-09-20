using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalChessMeet.Interfaces
{
    public interface IRegistrationRepository
    {
        void AddRegistration(Entities.Registration registration);
        void DeleteRegistration(Entities.Registration registration);
        bool CheckIfRegistered(string gd, string player_id);
    }
}
