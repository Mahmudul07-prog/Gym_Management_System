using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositroies.Interfaces
{
    public interface IPlanRepository
    {
        Plan? GetById(int id);
        IEnumerable<Plan> GetAll();
        int Update(Plan plan);
    }
}
