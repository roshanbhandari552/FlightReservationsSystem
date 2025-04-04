using Azure.Identity;
using FlightReservationSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using WebApplication1.Models;

namespace FlightReservationSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            var model = new Employee();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Employee model)
        {
            if (ModelState.IsValid)
            {
               var user = new ApplicationUser { FirstName = model.FirstName, UserName = model.Email };
               var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    /*ViewBag.successMessage = "Registration Successfully";*/
                    TempData["SuccessMessage"] = "Registration Successful";

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Register");
                }

                foreach(var error in result.Errors){
                    ModelState.AddModelError("", error.Description);
                }
                // Save the employee or register logic here
                // You can use _context.Employees.Add(model); if using EF
                return View(); // or wherever you want to go
            }

            return View(model); // If validation fails, re-show form with errors
        }
    }
}
