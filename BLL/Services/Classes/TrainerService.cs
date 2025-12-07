using BLL.Services.Interfaces;
using BLL.ViewModels;
using DataAccess.Models;
using DataAccess.Repositroies.Classes;
using DataAccess.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Classes
{
    public class TrainerService(IGenericRepository<Trainer> _trainerRepo,
                                IGenericRepository<Session> _sessionRepo,
                                IUnitOfWork _unitOfWork) : ITrainerService
    {
        public bool CreateTrainer(CreateTrainerViewModel createdTrainer)
        {
            try
            {
                if (IsEmailExist(createdTrainer.Email) || IsPhoneExist(createdTrainer.Phone)) return false;

                var trainer = new Trainer()
                {
                    Name = createdTrainer.Name,
                    Email = createdTrainer.Email,
                    Phone = createdTrainer.Phone,
                    Gender = createdTrainer.Gender,
                    DateOfBirth = createdTrainer.DateOfBirth,
                    Address = new Address()
                    {
                        Street = createdTrainer.Street,
                        City = createdTrainer.City,
                        BuildingNumber = createdTrainer.BuildingNumber
                    },
                    Specialites = createdTrainer.Specialites,
                    CreatedAt = DateTime.Now
                };
                _trainerRepo.Add(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public IEnumerable<TrainerViewModel> GetAllTrainer()
        {
            var Trainers = _trainerRepo.GetAll();
            if (Trainers is null || !Trainers.Any())
                return [];

            var vm = Trainers.Select(x => new TrainerViewModel()
            {
                Email = x.Email,
                Id = x.Id,
                Name = x.Name,
                Phone = x.Phone,
                Specialization = x.Specialites.ToString(),
            });

            return vm;
        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            Trainer? trainer = _trainerRepo.GetById(trainerId);
            if (trainer is null) return null;
            var trainerVM = new TrainerViewModel()
            {
                Id = trainerId,
                Name = trainer.Name,
                Phone = trainer.Phone,
                Email = trainer.Email,
                DateOfBirth = trainer.DateOfBirth.ToString(),
                Address = $"{trainer.Address.City}-{trainer.Address.Street}-{trainer.Address.BuildingNumber}",
            };
            return trainerVM;
        }

        public UpdatedTrainerViewModel? GetTrainerToUpdate(int trainerId)
        {
            var trainer = _trainerRepo.GetById(trainerId);
            if (trainer is null) return null;
            return new UpdatedTrainerViewModel()
            {
                Name = trainer.Name,
                Phone = trainer.Phone,
                Email = trainer.Email,
                BuildingNumber = trainer.Address.BuildingNumber,
                City = trainer.Address.City,
                Street = trainer.Address.Street,
                Specialites = trainer.Specialites
            };
        }

        public bool UpdateTrainer(int trainerId, UpdatedTrainerViewModel updatedTrainer)
        {
            try
            {
                var trainer = _trainerRepo.GetById(trainerId);

                if (trainer is null) return false;
                if ((IsEmailExist(updatedTrainer.Email) && trainer.Email != updatedTrainer.Email)
                || (IsPhoneExist(updatedTrainer.Phone) && trainer.Phone != updatedTrainer.Phone)) return false;

                trainer.Email = updatedTrainer.Email;
                trainer.Phone = updatedTrainer.Phone;
                trainer.Address.BuildingNumber = updatedTrainer.BuildingNumber;
                trainer.Address.City = updatedTrainer.City;
                trainer.Address.Street = updatedTrainer.Street;
                trainer.Specialites = updatedTrainer.Specialites;

                _trainerRepo.Update(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteTrainer(int trainerId)
        {
            var trainer = _trainerRepo.GetById(trainerId);
            if (trainer is null) return false;
            var futureSessions = _sessionRepo.GetAll(x => x.TrainerId == trainerId && x.EndDate > DateTime.Now);
            if (futureSessions.Any()) return false;

            _trainerRepo.Delete(trainer);
            return _unitOfWork.SaveChanges() > 0;

        }

        #region Helper Methods
        private bool IsEmailExist(string email)
        {
            return _trainerRepo.GetAll(X => X.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _trainerRepo.GetAll(X => X.Phone == phone).Any();
        }
        #endregion
    }
}
