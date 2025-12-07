using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositroies.Interfaces
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        IEnumerable<Session> GetAllSessionsWithIncludes();
        int GetCountOfBookedSlots(int SessionId);
        Session? GetSessionWithIncludes(int SessionId);
    }
}
