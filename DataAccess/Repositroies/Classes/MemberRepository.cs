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
    public class MemberRepository(GymSystemDbContext _dbContext) : GenericRepository<Member>(_dbContext), IMemberRepository
    {
        public IEnumerable<Member?> GetAllMember(Func<Member, bool>? condition = null)
        {
            return _dbContext.Members.Include(X => X.MemberShips)
                              .Include(X => X.MemberSessions)
                              .Where(condition!).ToList();
        }
    }
}
