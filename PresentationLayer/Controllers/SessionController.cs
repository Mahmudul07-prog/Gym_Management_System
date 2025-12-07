using BLL.Services.Interfaces;
using BLL.ViewModels.SessionsViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PresentationLayer.Controllers
{
    public class SessionController(ISessionService _sessionService) : Controller
    {
        #region Get All
        public IActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }
        #endregion

        #region Details
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var Sessions = _sessionService.SessionById(id);
            if (Sessions == null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Sessions);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            LoadDropDownsForTrainers();
            LoadDropDownsForCategories();
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateSessionViewModel createSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsForTrainers();
                LoadDropDownsForCategories();
                return View(createSession);
            }
            var result = _sessionService.CreateSession(createSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Created Succesfully !";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Faild To Create Member !";
                LoadDropDownsForTrainers();
                LoadDropDownsForCategories();
                return View(createSession);
            }
        }
        #endregion

        #region Edit
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionToUpdate(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDownsForTrainers();
            return View(Session);
        }
        [HttpPost]
        public IActionResult Edit(int id, UpdateSessionViewModel updateSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsForTrainers();
                return View(updateSession);
            }
            var result = _sessionService.UpdateSession(updateSession, id);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Updated Succesfully !";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Faild To Update Member !";
                LoadDropDownsForTrainers();
                return View(updateSession);
            }
        }

        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.SessionById(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View(Session);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _sessionService.DeleteSession(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Deleted Succesfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Faild To Delete Member !";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion



        #region Helper Methods
        private void LoadDropDownsForTrainers()
        {
            var Trainers = _sessionService.GetTrainers();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        private void LoadDropDownsForCategories()
        {
            var Categories = _sessionService.GetCategories();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }

        #endregion

    }
}
