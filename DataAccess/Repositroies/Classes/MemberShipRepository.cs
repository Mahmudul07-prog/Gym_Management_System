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
    public class MemberShipRepository(GymSystemDbContext _dbContext) : GenericRepository<MemberShip>(_dbContext), IMemberShipRepository
    {
        public IEnumerable<MemberShip> GetAll()
        {
            return _dbContext.MemberShips.Include(X => X.Plan)
                                         .Include(X => X.Member)
                                         .ToList();
        }
    }
}
