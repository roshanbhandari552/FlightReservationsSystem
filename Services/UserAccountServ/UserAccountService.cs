using FlightReservationSystem.Models;
using FlightReservationSystem.Repositories.UserAccountRepo;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using WebApplication1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FlightReservationSystem.Services.UserAccountServ
{
    public class UserAccountService : IUserAccountService
    {

        private readonly IUserAccountRepository _userRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserAccountService(IUserAccountRepository userRepository, SignInManager<ApplicationUser> signInManager)
        {
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        public List<ApplicationUser> GetUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public async Task<EditUserViewModel?> GetEditUserViewModelAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return null;

            return new EditUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                Email = user.UserName
            };
        }

        public async Task<(bool IsSuccessful, IEnumerable<string> Errors)> RegisterUserAsync(RegisterViewModel model)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await _userRepository.CreateUserAsync(user, model.Password);

            return result.Succeeded
                ? (true, Enumerable.Empty<string>())
                : (false, result.Errors.Select(e => e.Description));
        }

        public async Task<(bool IsSuccessful, IEnumerable<string> Errors)> UpdateUserAsync(EditUserViewModel model)
        {
            var user = await _userRepository.GetUserByIdAsync(model.Id);
            if (user == null)
                return (false, new[] { "User not found." });

            user.FirstName = model.FirstName;
            user.UserName = model.Email;
            user.Email = model.Email;

            var result = await _userRepository.UpdateUserAsync(user);

            return result.Succeeded
                ? (true, Enumerable.Empty<string>())
                : (false, result.Errors.Select(e => e.Description));
        }

        public async Task<(bool IsSuccessful, IEnumerable<string> Errors)> DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return (false, new[] { "User not found." });

            var result = await _userRepository.DeleteUserAsync(user);
            return result.Succeeded
                ? (true, Enumerable.Empty<string>())
                : (false, result.Errors.Select(e => e.Description));
        }

        public async Task<List<ApplicationUser>> SearchUsersAsync(string query)
        {
            return await _userRepository.SearchUsersAsync(query);
        }

        public async Task<(bool IsSuccessful, string ErrorMessage)> SignInAsync(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
                return (true, string.Empty);

            return (false, "Invalid login attempt.");
        }

        public AuthenticationProperties ConfigureGoogleLogin(string returnUrl)
        {
            var redirectUrl = $"/Account/GoogleResponse?returnUrl={Uri.EscapeDataString(returnUrl)}";
            return _signInManager.ConfigureExternalAuthenticationProperties(GoogleDefaults.AuthenticationScheme, redirectUrl);
        }

        public async Task<(bool Success, string RedirectUrl)> HandleGoogleResponseAsync(string returnUrl)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return (false, "/Account/Login");

            // Try sign-in
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
                return (true, returnUrl);

            // Create new user if doesn't exist
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);

            if (string.IsNullOrEmpty(email))
                return (false, "/Account/Login");

            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = firstName,
                    LastName = lastName
                };

                var createResult = await _userRepository.CreateUserAsync(user, password: null);
                if (!createResult.Succeeded)
                    return (false, "/Account/Login");

                var addLoginResult = await _userRepository.AddLoginAsync(user, info);
                if (!addLoginResult.Succeeded)
                    return (false, "/Account/Login");
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return (true, returnUrl);
        }

        public async Task LogoutAsync(HttpContext context)
        {
            await _signInManager.SignOutAsync();
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }


    }
}
