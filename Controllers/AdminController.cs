using Microsoft.AspNetCore.Mvc;

namespace FlightReservationSystem.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
