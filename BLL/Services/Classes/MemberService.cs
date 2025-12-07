using BLL.Services.AttachmentService;
using BLL.Services.Interfaces;
using BLL.ViewModels;
using DataAccess.Data.Contexts;
using DataAccess.Models;
using DataAccess.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Classes
{
    public class MemberService(IUnitOfWork _unitOfWork,
                               IAttachmentService _attachmentService) : IMemberService
    {
        public bool CreateMember(CreateMemberViewModel createdMember)
        {
            try
            {
                if (IsEmailExist(createdMember.Email) || IsPhoneExist(createdMember.Phone)) return false;

                var PhotoName = _attachmentService.Upload("Members", createdMember.Photo);
                if (PhotoName == null) return false;

                var member = new Member()
                {
                    Name = createdMember.Name,
                    Email = createdMember.Email,
                    Phone = createdMember.Phone,
                    Gender = createdMember.Gender,
                    DateOfBirth = createdMember.DateOfBirth,
                    Address = new Address()
                    {
                        Street = createdMember.Street,
                        City = createdMember.City,
                        BuildingNumber = createdMember.BuldingNumber
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createdMember.HealthViewModel.Height,
                        Weight = createdMember.HealthViewModel.Weight,
                        BloodType = createdMember.HealthViewModel.BloodType,
                        Note = createdMember.HealthViewModel.Note
                    },
                    Photo = PhotoName
                };
                _unitOfWork.GetRepository<Member>().Add(member);
                var IsCreated = _unitOfWork.SaveChanges() > 0;
                if (!IsCreated)
                {
                    _attachmentService.Delete(PhotoName, "Members");
                    return false;
                }
                return IsCreated;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (members is null || !members.Any()) return [];
            var vm = members.Select(X => new MemberViewModel
            {
                Id = X.Id,
                Name = X.Name,
                Email = X.Email,
                Phone = X.Phone,
                Photo = X.Photo,
                Gender = X.Gender.ToString()
            });
            return vm;
        }

        public MemberViewModel? GetMemberDetails(int MemberId)
        {
            // IPlanRepo
            // Inject PlanRepo && Memberhip Repo
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (Member is null) return null;

            var ViewModel = new MemberViewModel()
            {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Photo = Member.Photo,
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Gender = Member.Gender.ToString(),
                Address = $"{Member.Address.BuildingNumber} - {Member.Address.Street} - {Member.Address.City}"
            };
            var ActiveMemberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.MemberId == MemberId && X.Status == "Active").FirstOrDefault();

            if (ActiveMemberShip is not null)
            {
                ViewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                ViewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();

                // Plans
                var Plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
                ViewModel.PlanName = Plan?.Name;
            }
            return ViewModel;
        }

        public HealthViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (memberHealthRecord is null) return null;

            return new HealthViewModel()
            {
                Height = memberHealthRecord.Height,
                Weight = memberHealthRecord.Weight,
                BloodType = memberHealthRecord.BloodType,
                Note = memberHealthRecord.Note
            };
        }

        public MemberToUpdateViewModel GetMemberToUpdate(int MemberId)
        {
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            return new MemberToUpdateViewModel()
            {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Photo = Member.Photo,
                BuildingNumber = Member.Address.BuildingNumber,
                Street = Member.Address.Street,
                City = Member.Address.City,
            };
        }

        public bool RemoveMember(int MemberId)
        {
            var memberRepo = _unitOfWork.GetRepository<Member>();
            var MembeerSessionRepo = _unitOfWork.GetRepository<MemberSession>();
            var MemberShipRepo = _unitOfWork.GetRepository<MemberShip>();

            var Member = memberRepo.GetById(MemberId);
            if (Member is null) return false;

            //var HasActiveSession = MembeerSessionRepo.GetAll(X => X.MemberId == MemberId && X.Session.StartDate > DateTime.Now).Any();

            // Include (in Another Way)
            var sessionIds = _unitOfWork.GetRepository<MemberSession>()
                                           .GetAll(X => X.MemberId == MemberId).Select(X => X.SessionId);

            var HasActiveSession = _unitOfWork.GetRepository<Session>()
                                                .GetAll(X => sessionIds.Contains(X.Id) && X.StartDate > DateTime.Now).Any();

            if (HasActiveSession) return false;

            var Membeerships = MemberShipRepo.GetAll(X => X.MemberId == MemberId);

            try
            {
                if (Membeerships.Any())
                {
                    foreach (var membeership in Membeerships)
                        MemberShipRepo.Delete(membeership);
                }
                memberRepo.Delete(Member);
                var IsDeleted = _unitOfWork.SaveChanges() > 0;
                if (IsDeleted)
                    _attachmentService.Delete(Member.Photo, "Members");


                return IsDeleted;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool UpdateMemberDetails(int id, MemberToUpdateViewModel UpdatedMember)
        {
            try
            {
                var repo = _unitOfWork.GetRepository<Member>();

                var emailExists = repo.GetAll(x => x.Email == UpdatedMember.Email && x.Id != id);
                var phoneExists = repo.GetAll(x => x.Phone == UpdatedMember.Phone && x.Id != id);

                if (emailExists.Any() || phoneExists.Any()) return false;

                var Member = repo.GetById(id);
                if (Member is null) return false;
                Member.Email = UpdatedMember.Email;
                Member.Phone = UpdatedMember.Phone;
                Member.Address.BuildingNumber = UpdatedMember.BuildingNumber;
                Member.Address.Street = UpdatedMember.Street;
                Member.Address.City = UpdatedMember.City;
                Member.UpdatedAt = DateTime.Now;

                repo.Update(Member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }


        #region Helper Methods
        private bool IsEmailExist(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == phone).Any();
        }
        #endregion
    }
}
