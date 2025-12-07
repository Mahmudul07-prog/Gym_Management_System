using DataAccess.Data.Contexts;
using DataAccess.Models;
using DataAccess.Repositroies.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositroies.Classes
{
    public class SessionRepository(GymSystemDbContext _dbContext) : GenericRepository<Session>(_dbContext), ISessionRepository
    {
        public IEnumerable<Session> GetAllSessionsWithIncludes()
        {
            return _dbContext.Sessions.Include(X => X.SessionTrainer)
                                      .Include(X => X.SessionCategory)
                                      .ToList();
        }

        public int GetCountOfBookedSlots(int SessionId)
        {
            return _dbContext.MemberSessions.Count(X => X.SessionId == SessionId);
        }

        public Session? GetSessionWithIncludes(int SessionId)
        {
            return _dbContext.Sessions.Include(X => X.SessionTrainer)
                                      .Include(X => X.SessionCategory)
                                      .FirstOrDefault(X => X.Id == SessionId);
        }
    }
}
