using BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class HomeController(IAnalyticsService _analyticsService) : Controller
    {
        public IActionResult Index()
        {
            var Analytics = _analyticsService.GetAnalyticsData();
            return View(Analytics);
        }
    }
}
