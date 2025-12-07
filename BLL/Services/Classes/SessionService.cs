using AutoMapper;
using BLL.Services.Interfaces;
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
    public class SessionService(IUnitOfWork _unitOfWork,
                                IMapper _mapper) : ISessionService
    {
        public bool CreateSession(CreateSessionViewModel createdSession)
        {
            try
            {
                // Check for: 
                // Trainer Is Exist
                // Category Is Exist
                // Check StartDate Before EndDate

                if (!IsTrainerExist(createdSession.TrainerId)) return false;
                if (!IsCategoryExist(createdSession.CategoryId)) return false;
                if (!IsDateTimeValid(createdSession.StartDate, createdSession.EndDate)) return false;
                if (createdSession.Capacity > 25 || createdSession.Capacity < 0) return false;

                var Session = _mapper.Map<Session>(createdSession);
                _unitOfWork.GetRepository<Session>().Add(Session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {

            return _unitOfWork.SessionRepository.GetAllSessionsWithIncludes()?.Select(s => new SessionViewModel
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

        public SessionViewModel? SessionById(int sessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetSessionWithIncludes(sessionId);
            if (Session == null) return null;

            var MappedSession = _mapper.Map<SessionViewModel>(Session);
            MappedSession.AvailableSlots = MappedSession.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(MappedSession.Id);
            return MappedSession;
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var Session = _unitOfWork.GetRepository<Session>().GetById(sessionId);
            if (!IsSessionAvailableForUpdate(Session!)) return null;

            return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(UpdateSessionViewModel updateSession, int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForUpdate(Session!)) return false;
                if (!IsTrainerExist(updateSession.TrainerId)) return false;
                if (!IsDateTimeValid(updateSession.StartDate, updateSession.EndDate)) return false;

                _mapper.Map(updateSession, Session);
                Session!.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.GetRepository<Session>().Update(Session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteSession(int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (Session == null) return false;
                if (!IsSessionAvailableForDelete(Session!)) return false;

                _unitOfWork.SessionRepository.Delete(Session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerAndCategorySelectViewModel> GetTrainers()
        {
            var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            return _mapper.Map<IEnumerable<TrainerAndCategorySelectViewModel>>(Trainers);
        }

        public IEnumerable<TrainerAndCategorySelectViewModel> GetCategories()
        {
            var Categories = _unitOfWork.GetRepository<Category>().GetAll();
            return _mapper.Map<IEnumerable<TrainerAndCategorySelectViewModel>>(Categories);
        }

        #region Helper Methods
        private bool IsTrainerExist(int TrainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        }
        private bool IsCategoryExist(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }
        private bool IsDateTimeValid(DateTime StartDate, DateTime EndDate)
        {
            return StartDate < EndDate;
        }
        private bool IsSessionAvailableForUpdate(Session session)
        {
            if (session == null) return false;

            if (session.EndDate < DateTime.Now) return false;

            if (session.StartDate <= DateTime.Now) return false;

            var ActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (ActiveBooking) return true;
            return true;
        }
        private bool IsSessionAvailableForDelete(Session session)
        {
            if (session == null) return false;
            //// Complated => cannot delete
            //if (session.EndDate < DateTime.Now) return false;
            // UpComing => cannot delete
            if (session.StartDate > DateTime.Now) return false;
            // Started => cannot delete
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;
            // Has Active Booking => cannot delete
            var ActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (ActiveBooking) return true;
            return true;
        }

        #endregion
    }
}
