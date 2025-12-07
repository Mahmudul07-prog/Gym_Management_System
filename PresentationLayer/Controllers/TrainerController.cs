using BLL.Services.Classes;
using BLL.Services.Interfaces;
using BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController(ITrainerService _trainerService) : Controller
    {
        #region Get All
        public IActionResult Index()
        {
            var Trainers = _trainerService.GetAllTrainer();
            return View(Trainers);
        }

        #endregion

        #region Details
        public IActionResult TrainerDetails(int id)
        {
            var trainer = _trainerService.GetTrainerDetails(id);
            return View(trainer);
        }
        #endregion

        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateTrainerViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Mising Fields");
                return View(viewModel);
            }

            var result = _trainerService.CreateTrainer(viewModel);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Created Succesfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Faild To Create Trainer !";
            }
            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region Edit
        public IActionResult Edit(int id)
        {
            var trainer = _trainerService.GetTrainerToUpdate(id);
            return View(trainer);
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, UpdatedTrainerViewModel updatedTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
                return View(updatedTrainer);
            }

            var result = _trainerService.UpdateTrainer(id, updatedTrainer);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully !";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Faild To Update Trainer !";
            return View(updatedTrainer);
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id Cannot Be 0 Or Negative Number !";
                return RedirectToAction(nameof(Index));
            }
            var Trainer = _trainerService.GetTrainerDetails(id);
            if (Trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found !";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = Trainer.Id;
            ViewBag.TrainerName = Trainer.Name;

            return View(Trainer);
        }
        [HttpPost]
        public IActionResult DeleteConfirmed([FromForm] int id)
        {
            var result = _trainerService.DeleteTrainer(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Removed Successfully !";
            }
            else
            {
                TempData["ErrorMessage"] = "Faild To Remove Trainer !";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
