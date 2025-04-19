using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlightReservationSystem.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager) { 
            _roleManager = roleManager;
            _userManager = userManager;
        }    
        [HttpGet]
        [Authorize]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole()
                {
                    Name = model.Role
                };
                IdentityResult result = await _roleManager.CreateAsync(role);

                if (result.Succeeded){
                    return RedirectToAction("ListRole", "Administration");
                }
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

            }
            return View(model);

        
        }

        [HttpGet]
        public IActionResult ListRole()
        {
           var role = _roleManager.Roles.ToList();
            return View(role);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(String id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) {
                ViewBag.Error = $"Role {id} is not found";
                return View("NotFound");
            }

            var model = new EditViewModel
            {
                Id = role.Id,
                Name = role.Name
            };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }
            return View(model);
            
        }
    }
}
