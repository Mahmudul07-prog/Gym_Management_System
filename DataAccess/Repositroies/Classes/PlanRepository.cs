using DataAccess.Data.Contexts;
using DataAccess.Models;
using DataAccess.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositroies.Classes
{
    public class PlanRepository(GymSystemDbContext _dbContext) : IPlanRepository
    {
        public IEnumerable<Plan> GetAll()
        {
            return _dbContext.Plans.ToList();
        }

        public Plan? GetById(int id)
        {
            return _dbContext.Plans.Find(id);
        }

        public int Update(Plan plan)
        {
           _dbContext.Plans.Update(plan);
            return _dbContext.SaveChanges();
        }
    }
}
