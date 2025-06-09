using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Authentication;

namespace FlightReservationSystem.Services.UserAccountServ
{
    public interface IUserAccountService
    {

        List<ApplicationUser> GetUsers();
        Task<EditUserViewModel?> GetEditUserViewModelAsync(string id);
        Task<(bool IsSuccessful, IEnumerable<string> Errors)> RegisterUserAsync(RegisterViewModel model);

        Task<(bool IsSuccessful, IEnumerable<string> Errors)> UpdateUserAsync(EditUserViewModel model);
        Task<(bool IsSuccessful, IEnumerable<string> Errors)> DeleteUserAsync(string id);
        Task<List<ApplicationUser>> SearchUsersAsync(string query);
        Task<(bool IsSuccessful, string ErrorMessage)> SignInAsync(LoginViewModel model);

        AuthenticationProperties ConfigureGoogleLogin(string returnUrl);
        Task<(bool Success, string RedirectUrl)> HandleGoogleResponseAsync(string returnUrl);
        Task LogoutAsync(HttpContext context);


    }

}
