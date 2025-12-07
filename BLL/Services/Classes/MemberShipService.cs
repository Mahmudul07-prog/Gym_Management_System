using AutoMapper;
using BLL.Services.Interfaces;
using BLL.ViewModels;
using DataAccess.Models;
using DataAccess.Repositroies.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Classes
{
    public class MemberShipService(IUnitOfWork _unitOfWork,
                                   IMapper _mapper) : IMemberShipService
    {
        public IEnumerable<MemberShipViewModel?> GetAll()
        {
            var MemberShips = _unitOfWork.MemberShipRepository.GetAll();
            //foreach (var membership in MemberShips)
            //{
            //    if (membership.Status == "Expired !")
            //    {
            //        _unitOfWork.GetRepository<MemberShip>().Delete(membership);
            //    }
            //}

            return _mapper.Map<IEnumerable<MemberShipViewModel>>(MemberShips);
        }

        bool IMemberShipService.CreateMemberShip(CreatedMemberShipViewModel viewModel)
        {
            try
            {
                var Member = _unitOfWork.GetRepository<Member>().GetById(viewModel.MemberId);
                if (Member is null) return false;
                var Plan = _unitOfWork.GetRepository<Plan>().GetById(viewModel.PlanId);
                if (Plan is null) return false;

                var memberShipRepo = _unitOfWork.GetRepository<MemberShip>();
                var memberHasMemberShip = memberShipRepo.GetAll(x => x.MemberId == viewModel.MemberId).FirstOrDefault();
                if (memberHasMemberShip is not null)
                    return false;

                var MemberShip = new MemberShip()
                {
                    MemberId = viewModel.MemberId,
                    PlanId = viewModel.PlanId,
                    CreatedAt = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(Plan.DurationDays),
                    UpdatedAt = DateTime.Now
                };

                memberShipRepo.Add(MemberShip);
                var result = _unitOfWork.SaveChanges() > 0;

                if (!result) return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CancelMemberShip(MemberShipToCancelViewModel viewModel)
        {
            var repo = _unitOfWork.GetRepository<MemberShip>();
            var memberShip = repo.GetAll(M => M.MemberId == viewModel.MemberId && M.PlanId == viewModel.PlanId).FirstOrDefault();
            if (memberShip is null) return false;
            _unitOfWork.GetRepository<MemberShip>().Delete(memberShip);
            var result = _unitOfWork.SaveChanges() > 0;
            if (!result) return false;
            return true;
        }

        public IEnumerable<MembersAndPlansDropListViewModel?> GetMembers()
        {
            var Members = _unitOfWork.MemberRepository.GetAllMember(X => !X.MemberShips.Any());
            return _mapper.Map<IEnumerable<MembersAndPlansDropListViewModel>>(Members);
        }

        public IEnumerable<MembersAndPlansDropListViewModel?> GetPlans()
        {
            var Plans = _unitOfWork.GetRepository<Plan>().GetAll();
            return _mapper.Map<IEnumerable<MembersAndPlansDropListViewModel>>(Plans);
        }
    }
}
