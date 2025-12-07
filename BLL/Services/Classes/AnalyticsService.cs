using BLL.Services.Interfaces;
using BLL.ViewModels;
using DataAccess.Models;
using DataAccess.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Classes
{
    public class AnalyticsService(IUnitOfWork _unitOfWork) : IAnalyticsService
    {
        public AnalyticsViewModel GetAnalyticsData()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAll();
            return new AnalyticsViewModel
            {
                ActiveMembers = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.Status == "Active").Count(),
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                Trainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpComingSessions = Sessions.Count(X => X.StartDate > DateTime.Now),
                OnGoingSessions = Sessions.Count(X => X.StartDate <= DateTime.Now && X.EndDate >= DateTime.Now),
                ComplatedSessions =Sessions.Count(X => X.EndDate < DateTime.Now)
            };
        }
    }
}
