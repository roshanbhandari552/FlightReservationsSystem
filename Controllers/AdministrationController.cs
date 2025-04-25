using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using System.Data;
using System.Reflection.Metadata.Ecma335;

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

        [HttpPost]
        public async Task<IActionResult> EditRole(EditViewModel model)
        {

            var role = await _roleManager.FindByIdAsync(model.Id);
           


           if(role == null)
            {
                ViewBag.Error = $"{role} is null ";
                return View("NotFound");
            }

            else
            {
                role.Name = model.Name;
               var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRole", "Administration");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

        }
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(String roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var model = new List<UserRoleViewModel>();
            if (role == null)
            {
                ViewBag.Error = $"{role} is null ";
                return View("NotFound");
            }

            ViewBag.RoleId = role.Id;
            foreach (var user in _userManager.Users) {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    IsSelected = await _userManager.IsInRoleAsync(user, role.Name)
                };

            model.Add(userRoleViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if(role == null)
            {
                ViewBag.Error = $"{role} is null ";
                return View("NotFound");
            }

            for(int i = 0; i< model.Count; i ++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);
               

                IdentityResult result = null;

                // Add user to role if selected and not already in role
                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                // Remove user from role if unselected and currently in role
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    // No change needed
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }

            }
            return RedirectToAction("EditRole", new { Id = roleId });
        }
           
           

    }
}
