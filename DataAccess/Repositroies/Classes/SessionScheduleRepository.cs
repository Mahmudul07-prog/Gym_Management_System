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
    public class SessionScheduleRepository(GymSystemDbContext _dbContext) : GenericRepository<MemberSession>(_dbContext), ISessionScheduleRepository
    {
        public IEnumerable<MemberSession?> GetAllWithIncludes()
        {
            return _dbContext.MemberSessions.Include(ms => ms.Session)
                                            .Include(ms => ms.Member)
                                            .ToList();
        }
    }
}
