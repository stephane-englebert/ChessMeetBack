using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalChessMeet.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<Entities.Member> GetMembers();
        void AddMember(Entities.Member member);
        bool ExistEmail(string email);
        bool ExistPseudo(string pseudo);
        Entities.MemberDetails GetMemberById(int id);
        Entities.MemberDetails GetMemberByCredentials(string username, string password);
    };
}
