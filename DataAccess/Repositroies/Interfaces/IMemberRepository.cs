using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositroies.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<Member?> GetAllMember(Func<Member, bool>? condition = null);
    }
}
