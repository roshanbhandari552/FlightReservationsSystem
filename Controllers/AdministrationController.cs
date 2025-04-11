using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlightReservationSystem.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdministrationController(RoleManager<IdentityRole> roleManager) { 
            _roleManager = roleManager;
        }    
        [HttpGet]
        [Authorize]
        public IActionResult CreateRole()
        {
            return View();
        }
    }
}
