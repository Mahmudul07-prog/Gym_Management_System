using BLL.Services.Interfaces;
using BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Controllers
{
    public class MemberShipController(IMemberShipService _memberShipService) : Controller
    {
        #region GetAll
        public IActionResult Index()
        {
            var memberShips = _memberShipService.GetAll();
            return View(memberShips);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            MembersDropList();
            PlansDropList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreatedMemberShipViewModel viewModel)
        {

            if (!ModelState.IsValid)
            {
                MembersDropList();
                PlansDropList();
                return View(viewModel);
            }
            var result = _memberShipService.CreateMemberShip(viewModel);
            if (result)
            {
                TempData["SuccessMessage"] = "MemberShip Created Succesfully !";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                MembersDropList();
                PlansDropList();
                TempData["ErrorMessage"] = "Faild To Create MemberShip !";
                return View(viewModel);
            }
        }
        #endregion

        #region Cancle
        [HttpPost]
        public IActionResult Cancel(MemberShipToCancelViewModel viewModel)
        {
            var result = _memberShipService.CancelMemberShip(viewModel);
            if (result)
            {
                TempData["SuccessMessage"] = "MemberShip Canceled Succesfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Faild To Canceled MemberShip !";
            }
            return RedirectToAction(nameof(Index));


        }
        #endregion

        #region Helper DropList Loding
        private void MembersDropList()
        {
            var Members = _memberShipService.GetMembers();
            ViewBag.Members = new SelectList(Members, "Id", "Name");
        }
        private void PlansDropList()
        {
            var Plans = _memberShipService.GetPlans();
            ViewBag.Plans = new SelectList(Plans, "Id", "Name");
        }

        #endregion
    }
}
