using Azure.Identity;
using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
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
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
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

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                // Save the employee or register logic here
                // You can use _context.Employees.Add(model); if using EF
                return View(); // or wherever you want to go
            }

            return View(model); // If validation fails, re-show form with errors
        }


        [HttpGet]
        public IActionResult User()
        {
            var user = _userManager.Users.ToList();
            return View(user);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> IsEmailAvailable(String email)
        { 
           var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email '{email}' is already taken");
            }
        }

            [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    /*ViewBag.successMessage = "Registration Successfully";*/


                    return RedirectToAction("Index");
                }


                ModelState.AddModelError(String.Empty, "Invalid Login");

                // Save the employee or register logic here
                // You can use _context.Employees.Add(model); if using EF
                return View(); // or wherever you want to go
            }

            return View(model); // If validation fails, re-show form with errors
        }

    }
}
