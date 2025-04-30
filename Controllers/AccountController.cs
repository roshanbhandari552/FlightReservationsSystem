using Azure.Identity;
using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
                var user = new ApplicationUser { FirstName = model.FirstName, Email = model.Email, UserName = model.Email };
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

        [HttpGet]
        public async Task<IActionResult> EditUser (string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                ViewBag.error = "Error";
                return View("NotFound", "Administration");
            }
            var editUserViewModel = new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Email = user.UserName
            };

            return View(editUserViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.error = "Error";
                return View("NotFound", "Administration");
            }
            else
            {
                user.FirstName = model.FirstName;
                user.UserName = model.Email;
                
                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("User");
                }
           
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                
                    }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser( string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if(user == null)
            {
                ViewBag.error = "Error";
                return View("NotFound", "Administration");
            }

           
                var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction("User");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return RedirectToAction("EditUser", new { id = id });


        }
        /*
                [HttpGet]
                public async Task<IActionResult> SearchUsers(string query)
                {
                    List<ApplicationUser> users;

                    if (!string.IsNullOrEmpty(query))
                    {
                        users = await _userManager.Users
                            .Where(u => u.UserName.Contains(query) || u.FirstName.Contains(query))
                            .ToListAsync();
                    }
                    else
                    {
                        users = await _userManager.Users.ToListAsync();
                    }

                    return PartialView("UserPartial", users); // 👈 reuse your existing view
                }*/

        [HttpGet]
        public async Task<IActionResult> SearchUsers(string query)
        {
            var users = string.IsNullOrWhiteSpace(query)
                ? await _userManager.Users.ToListAsync()
                : await _userManager.Users
                    .Where(u => u.UserName.Contains(query) || u.Email.Contains(query))
                    .ToListAsync();

            return PartialView("UserPartial", users);
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
      
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            Console.WriteLine("Return URL: " + returnUrl);

            if (ModelState.IsValid)
            {
               

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {


                        /*ViewBag.successMessage = "Registration Successfully";*/
                        return Redirect(returnUrl);
                    }                 
               
                    else
                    {
                        return RedirectToAction("Index", "home");
                    }
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
