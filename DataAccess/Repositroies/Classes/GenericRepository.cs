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
    public class GenericRepository<TEntity>(GymSystemDbContext _dbContext) : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        public IEnumerable<TEntity> GetAll(Func<TEntity, bool> condition)
        {
            if (condition is null) return _dbContext.Set<TEntity>().ToList();
            return _dbContext.Set<TEntity>().Where(condition).ToList();
        }

        public TEntity? GetById(int id) => _dbContext.Set<TEntity>().Find(id);
        public void Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
        }


        public void Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
        }
    }
}
