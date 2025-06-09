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
        private readonly IUserAccountService _userAccountService;
        public AccountController(UserManager<ApplicationUser> userManager, IUserAccountService userAccountService)
        {
            _userManager = userManager;
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
            var user = _userAccountService.GetUsers();
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser (string id)
        {
            var viewModel = await _userAccountService.GetEditUserViewModelAsync(id);
            if (viewModel == null)
            {
                ViewBag.error = "Error";
                return View("NotFound", "Administration");
            }
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var (IsSuccessful, Errors) = await _userAccountService.UpdateUserAsync(model);

            if (IsSuccessful)
            {
                TempData["SuccessMessage"] = "User updated successfully.";
                return RedirectToAction("User");
            }

            foreach (var error in Errors)
                ModelState.AddModelError("", error);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var (IsSuccessful, Errors) = await _userAccountService.DeleteUserAsync(id);

            if (IsSuccessful)
                return RedirectToAction("User");

            foreach (var error in Errors)
                ModelState.AddModelError("", error);

            return RedirectToAction("EditUser", new { id });
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
            var users = await _userAccountService.SearchUsersAsync(query);
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

            if (!ModelState.IsValid)
                return View(model);

            var (IsSuccessful, ErrorMessage) = await _userAccountService.SignInAsync(model);

            if (IsSuccessful)
                return RedirectToLocal(returnUrl);

            ModelState.AddModelError(string.Empty, ErrorMessage);
            return View(model);
        }

        [HttpGet]
        public IActionResult GoogleLogin(string returnUrl = "/")
        {
            var props = _userAccountService.ConfigureGoogleLogin(returnUrl);
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            var (success, redirectUrl) = await _userAccountService.HandleGoogleResponseAsync(returnUrl);
            return Redirect(redirectUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await _userAccountService.LogoutAsync(HttpContext);
            return RedirectToAction("Login", "Account");
        }


    }
}
