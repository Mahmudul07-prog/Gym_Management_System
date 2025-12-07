using BLL.Services.Interfaces;
using BLL.ViewModels;
using DataAccess.Models;
using DataAccess.Repositroies.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class MemberController(IMemberService _memberService) : Controller
    {
        #region GetAll
        public IActionResult Index()
        {
            var Members = _memberService.GetAllMembers();
            return View(Members);
        }
        #endregion

        #region Details
        public IActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }

            var Member = _memberService.GetMemberDetails(id);
            if (Member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }

        #endregion

        #region HealthRecordDetails
        public IActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }
            var healthRecord = _memberService.GetMemberHealthRecordDetails(id);
            if (healthRecord == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateMember(CreateMemberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Mising Fields");
                return View(nameof(Create), model);
            }
            bool result = _memberService.CreateMember(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Succesfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Faild To Create Member !";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Edit
        public IActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }

            var Member = _memberService.GetMemberToUpdate(id);
            if (Member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }
        [HttpPost]
        public IActionResult MemberEdit([FromRoute] int id, MemberToUpdateViewModel MemberToUpdate)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
                return View(MemberToUpdate);
            }
            var result = _memberService.UpdateMemberDetails(id, MemberToUpdate);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Faild To Update Member !";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberService.GetMemberDetails(id);
            if (Member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            ViewBag.MemberName = Member.Name;
            return View(Member);
        }
        public IActionResult DeleteConfirmed([FromForm]int id)
        {
            var result = _memberService.RemoveMember(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Removed Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Faild To Remove Member !";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
