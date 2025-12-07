using BLL.Services.Interfaces;
using BLL.ViewModels.PlanViewModels;
using DataAccess.Models;
using DataAccess.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Classes
{
    public class PlanService(IUnitOfWork _unitOfWork) : IPlanService
    {
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (Plans is null || !Plans.Any()) return [];

            return Plans.Select(p => new PlanViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DurationDays = p.DurationDays,
                IsActive = p.IsActive,
                Price = p.Price
            });
        }

        public PlanViewModel GetPlanById(int id)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (Plan is null) return null;

            return new PlanViewModel
            {
                Id = Plan.Id,
                Name = Plan.Name,
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                IsActive = Plan.IsActive,
                Price = Plan.Price
            };
        }

        public UpdatedPlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null || Plan.IsActive == false || HasActiveMemberShip(PlanId)) return null;

            return new UpdatedPlanViewModel()
            {
                PlanDescription = Plan.Description,
                PlanName = Plan.Name,
                DurationDays = Plan.DurationDays,
                Price = Plan.Price
            };
        }

        public bool UpdatePlan(int PlanId, UpdatedPlanViewModel updatedPlan)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan is null || HasActiveMemberShip(PlanId)) return false;

            try
            {
                (Plan.Description, Plan.DurationDays, Plan.Price, Plan.UpdatedAt) =
                (updatedPlan.PlanDescription, updatedPlan.DurationDays, updatedPlan.Price, DateTime.Now);

                _unitOfWork.GetRepository<Plan>().Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ToggleStatus(int PlanId)
        {
            var Repo = _unitOfWork.GetRepository<Plan>();
            var Plan = Repo.GetById(PlanId);
            if (Plan is null || HasActiveMemberShip(PlanId)) return false;

            Plan.IsActive = Plan.IsActive == true ? false : true;

            Plan.UpdatedAt = DateTime.Now;
            try
            {
                Repo.Update(Plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }



        #region Helper Methods
        public bool HasActiveMemberShip(int PlanId)
        {
            var ActiveMemberShip = _unitOfWork.GetRepository<MemberShip>()
                                                                 .GetAll(X => X.PlanId == PlanId && X.Status == "Active");
            return ActiveMemberShip.Any();
        }

        #endregion
    }
}
