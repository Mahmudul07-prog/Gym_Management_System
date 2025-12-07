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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymSystemDbContext _dbContext;
        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        public UnitOfWork(GymSystemDbContext dbContext,
                          ISessionRepository sessionRepository,
                          IMemberShipRepository memberShipRepository,
                          IMemberRepository memberRepository,
                          ISessionScheduleRepository sessionScheduleRepository)
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
            MemberShipRepository = memberShipRepository;
            MemberRepository = memberRepository;
            SessionScheduleRepository = sessionScheduleRepository;
        }

        public ISessionRepository SessionRepository { get; }

        public IMemberShipRepository MemberShipRepository { get; }

        public IMemberRepository MemberRepository { get; }

        public ISessionScheduleRepository SessionScheduleRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var EntityType = typeof(TEntity);
            if (_repositories.TryGetValue(EntityType, out var Repo))
                return (IGenericRepository<TEntity>)Repo;

            var NewRepo = new GenericRepository<TEntity>(_dbContext);
            _repositories[EntityType] = NewRepo;
            return NewRepo;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
    }
}
