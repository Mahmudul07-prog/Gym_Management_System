using BLL.Services.Interfaces;
using BLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class PlanController(IPlanService _planService) : Controller
    {
        #region Get All
        public IActionResult Index()
        {
            var Plans = _planService.GetAllPlans();
            return View(Plans);
        }
        #endregion

        #region Details
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Plan = _planService.GetPlanById(id);
            if (Plan == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }
        #endregion
        #region Edit
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 or Negative Number";
                return RedirectToAction(nameof(Index));
            }
            var Plan = _planService.GetPlanToUpdate(id);
            if (Plan == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, UpdatedPlanViewModel updatedPlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Again");
                return View(updatedPlan);
            }
            var result = _planService.UpdatePlan(id, updatedPlan);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully !";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Faild To Update Plan !";
            return View(updatedPlan);
        }
        #endregion

        [HttpPost]
        public IActionResult ToggleStatus(int id)
        {
            var result = _planService.ToggleStatus(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan Status Changed Successfully !";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Faild To Change Status Plan !";
            return RedirectToAction(nameof(Index));
        }
    }
}
