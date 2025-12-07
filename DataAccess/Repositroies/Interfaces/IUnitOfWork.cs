using DataAccess.Models;
using DataAccess.Repositroies.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositroies.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        int SaveChanges();

        public ISessionRepository SessionRepository { get;}
        public IMemberShipRepository MemberShipRepository { get;}
        public IMemberRepository MemberRepository { get;}
        public ISessionScheduleRepository SessionScheduleRepository { get;}

    }
}
