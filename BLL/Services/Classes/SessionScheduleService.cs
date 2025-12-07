using AutoMapper;
using BLL.Services.Interfaces;
using BLL.ViewModels;
using BLL.ViewModels.SessionsViewModel;
using DataAccess.Models;
using DataAccess.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Classes
{
    public class SessionScheduleService(IUnitOfWork _unitOfWork,
                                 IMapper _mapper) : ISessionScheduleService
    {


        #region New Booking
        public IEnumerable<GetMembersToNewBooking> membersToNewBookings(int SessionId)
        {
            var members = _unitOfWork.MemberRepository.GetAllMember(m => !m.MemberSessions.Any(m => m.SessionId == SessionId));
            return _mapper.Map<IEnumerable<GetMembersToNewBooking>>(members);
        }

        public bool AddNewBooking(CreatedSessionScheduleViewModel viewModel)
        {
            try
            {
                if (viewModel is null || viewModel.MemberId == 0 || viewModel.SessionId == 0) return false;

                //var sessionUpcoming = _unitOfWork.SessionRepository.GetAll(s => s.StartDate < s.EndDate && s.StartDate > DateTime.Now);
                //var sessionUpcoming = _unitOfWork.SessionRepository.GetAll(s => s.Id == viewModel.SessionId && s.StartDate < s.EndDate && s.StartDate > DateTime.Now).FirstOrDefault();

                if (CheckSessionUpcoming(viewModel.SessionId) != true) return false;

                var memberSession = new MemberSession()
                {
                    MemberId = viewModel.MemberId,
                    SessionId = viewModel.SessionId,
                    CreatedAt = DateTime.Now,
                    IsAttend = false,
                    UpdatedAt = DateTime.Now
                };

                _unitOfWork.GetRepository<MemberSession>().Add(memberSession);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public IEnumerable<SessionScheduleViewModel> GetAll()
        {
            var SessionSchedule = _unitOfWork.SessionScheduleRepository.GetAllWithIncludes();
            return _unitOfWork.SessionRepository.GetAllSessionsWithIncludes()?.Select(s => new SessionScheduleViewModel
            {
                Id = s.Id,
                Description = s.Description,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                Capacity = s.Capacity,
                TrainerName = s.SessionTrainer.Name,
                CategoryName = s.SessionCategory.CategoryName,
                AvailableSlots = s.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(s.Id)
            }) ?? [];
        }

        public bool updateIsAttend(CreatedSessionScheduleViewModel viewModel)
        {
            var Session = _unitOfWork.GetRepository<MemberSession>().GetAll(s => s.SessionId == viewModel.SessionId && s.MemberId == viewModel.MemberId).FirstOrDefault();
            if (Session is null) return false;
            Session.IsAttend = !Session.IsAttend;
            _unitOfWork.GetRepository<MemberSession>().Update(Session);
            return _unitOfWork.SaveChanges() > 0;

        }


        ////public IEnumerable<MembersViewModel?> GetMembers(int sessionId)
        ////{
        ////    var Members = _unitOfWork.MemberRepository.GetAllMember(M => !M.MemberSessions.Any(s => s.SessionId == sessionId));
        ////    return _mapper.Map<IEnumerable<MembersViewModel>>(Members);
        ////}

        ////public IEnumerable<MembersViewModel?> GetUpcomingSessions()
        ////{
        ////    var upcomingSession = _unitOfWork.SessionRepository.GetAll(s => s.StartDate < s.EndDate && s.StartDate > DateTime.Now);
        ////    return _mapper.Map<IEnumerable<MembersViewModel>>(upcomingSession);
        ////}

        public IEnumerable<MembersViewModel?> GetMembersForUpcomingSession(int sessionId)
        {
            if (CheckSessionUpcoming(sessionId) is not true)
                throw new Exception("Session is not UpComing");

            var Members = _unitOfWork.MemberRepository.GetAllMember(m => m.MemberSessions.Any(x => x.SessionId == sessionId));
            var vm = Members.Select(m => new MembersViewModel()
            {
                Id = m.Id,
                BookingDate = m.MemberSessions.Where(ms => ms.SessionId == sessionId).Select(ms => ms.CreatedAt.ToShortDateString()).FirstOrDefault()!,
                Name = m.Name
            });
            return vm;
            //return _mapper.Map<IEnumerable<MembersViewModel>>(Members);
        }

        public IEnumerable<MembersViewModel?> GetMembersForOngoingSessions(int sessionId)
        {
            if (CheckSessionOngoing(sessionId) is not true)
                throw new FieldAccessException();

            var Members = _unitOfWork.MemberRepository.GetAllMember(m => m.MemberSessions.Any(x => x.SessionId == sessionId));
            var memberVM = Members.Select(m => new MembersViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                IsAttend = m.MemberSessions.FirstOrDefault(x => x.SessionId == sessionId).IsAttend
            });
            return memberVM;
        }

        public bool CancelBooking(CancelMembersBookingViewModel viewModel)
        {
            if (viewModel is null) return false;
            var Repo = _unitOfWork.GetRepository<MemberSession>();
            var memberSession = Repo.GetAll(ms => ms.MemberId == viewModel.MemberId && ms.SessionId == viewModel.SessionId).FirstOrDefault();
            if (memberSession is null) return false;

            Repo.Delete(memberSession);
            return _unitOfWork.SaveChanges() > 0;
        }

        #region Helper Methods
        private bool CheckSessionUpcoming(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetAll(s => s.Id == sessionId && s.StartDate < s.EndDate && s.StartDate > DateTime.Now).FirstOrDefault();
            return session != null ? true : false;
        }
        private bool CheckSessionOngoing(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetAll(s => s.Id == sessionId && s.StartDate < s.EndDate && s.StartDate <= DateTime.Now).FirstOrDefault();
            return session != null ? true : false;
        }


        #endregion
    }
}
