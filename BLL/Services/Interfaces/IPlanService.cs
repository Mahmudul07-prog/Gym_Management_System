using BLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel GetPlanById(int id);
        UpdatedPlanViewModel? GetPlanToUpdate(int PlanId);
        bool UpdatePlan(int PlanId, UpdatedPlanViewModel updatedPlan);

        bool ToggleStatus(int PlanId);
    }
}
