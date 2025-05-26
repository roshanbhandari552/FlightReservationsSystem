using Azure.Identity;
using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using static FlightReservationSystem.Services.UserAccountServ.IUserAccountService;
using FlightReservationSystem.Services.UserAccountServ;

namespace FlightReservationSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserAccountService _userAccountService;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserAccountService userAccountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userAccountService = userAccountService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (IsSuccessful, Errors) = await _userAccountService.RegisterUserAsync(model);

            if (IsSuccessful)
            {
                TempData["SuccessMessage"] = "Registration Successful";
                return RedirectToAction("Register");
            }

            foreach (var error in Errors)
                ModelState.AddModelError(string.Empty, error);

            return View(model);
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

        //Return url
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // fallback if returnUrl is null or invalid
            return RedirectToAction("FlightSearch", "Flight");
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
                   return RedirectToLocal(returnUrl);
                }


                ModelState.AddModelError(String.Empty, "Invalid Login");
                return View();
            }

            return View(model); // If validation fails, re-show form with errors
        }

        [HttpGet]
        public IActionResult GoogleLogin(string returnUrl = null)
        {
            var redirectUrl = Url.Action("GoogleResponse", "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse(string returnUrl = null)
        {
            // Get login info from Google
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction("Login");

            // Try to log in the user if they already have an external login
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (signInResult.Succeeded)
                return RedirectToLocal(returnUrl);

            // If user doesn't exist yet, get email and create them
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);

            if (email != null)
            {
                // Check if user exists in DB by email
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName
                        // You can add name or other fields if needed
                    };

                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Failed to create user.");
                        return RedirectToAction("Login");
                    }

                    // Link Google login to the newly created user
                    await _userManager.AddLoginAsync(user, info);
                }

                // Sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }

            // If email is missing, fail
            ModelState.AddModelError(string.Empty, "Email claim not received.");
            return RedirectToAction("Login");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

    }
}
