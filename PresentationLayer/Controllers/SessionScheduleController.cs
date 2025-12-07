using BLL.Services.Interfaces;
using BLL.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Controllers
{
    public class SessionScheduleController(ISessionScheduleService _sessionSchedule) : Controller
    {
        #region GetAll
        public IActionResult Index()
        {
            var sessionSchedule = _sessionSchedule.GetAll();
            return View(sessionSchedule);
        }
        #endregion


        public IActionResult GetMembersForOnGoing(int id)
        {
            if (id <= 0) return RedirectToAction(nameof(Index));
            var Members = _sessionSchedule.GetMembersForOngoingSessions(id);
            ViewBag.SessionId = id;
            return View(Members);
        }
        public IActionResult GetMembersForUpcoming(int id)
        {
            if (id <= 0) return RedirectToAction(nameof(Index));
            var Members = _sessionSchedule.GetMembersForUpcomingSession(id);
            ViewBag.SessionId = id;
            return View(Members);
        }
        [HttpPost]
        public IActionResult CancelBooking(int SessionId, int MemberId)
        {
            if (MemberId <= 0 || SessionId <= 0) return RedirectToAction(nameof(GetMembersForUpcoming), new { id = SessionId });
            var CancelBookingVM = new CancelMembersBookingViewModel()
            {
                MemberId = MemberId,
                SessionId = SessionId
            };
            var result = _sessionSchedule.CancelBooking(CancelBookingVM);
            if (result)
            {
                TempData["SuccessMessage"] = "Member's Booked Canceled Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Can not Cancel Member's Booked !";
            }
            return RedirectToAction(nameof(GetMembersForUpcoming), new { id = SessionId });
        }

        public IActionResult ToggleAttednd(CreatedSessionScheduleViewModel viewModel)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(GetMembersForOnGoing), new { id = viewModel.SessionId });
            var result = _sessionSchedule.updateIsAttend(viewModel);
            if (result)
            {
                TempData["SuccessMessage"] = "Member's Attende !";
            }
            else
            {
                TempData["ErrorMessage"] = "Somthing Went Wrong !";
            }
            return RedirectToAction(nameof(GetMembersForOnGoing), new { id = viewModel.SessionId });
        }

        public IActionResult NewBooking(int id)
        {
            // membersToNewBookings
            ViewBag.Members = new SelectList(_sessionSchedule.membersToNewBookings(id), "Id", "Name");
            ViewBag.SessionId = id;
            return View();
        }
        [HttpPost]
        public IActionResult NewBooking(CreatedSessionScheduleViewModel viewModel)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(GetMembersForUpcoming), new { id = viewModel.SessionId });
            var result = _sessionSchedule.AddNewBooking(viewModel);
            if (result)
            {
                TempData["SuccessMessage"] = "Member's Added Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Somthing Went Wrong !";
            }
            return RedirectToAction(nameof(GetMembersForUpcoming), new { id = viewModel.SessionId });
        }
    }
}
