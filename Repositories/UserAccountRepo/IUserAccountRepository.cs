using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace FlightReservationSystem.Repositories.UserAccountRepo
{
    public interface IUserAccountRepository
    {
        Task Register(Employee model);
        Task<IActionResult> EditUser(string id);
        Task<IActionResult> EditUser(EditUserViewModel model, string id);
        Task<IActionResult> DeleteUser(string id);
        Task<IActionResult> SearchUsers(string query);
        Task<IActionResult> IsEmailAvailable(String email);
        Task<IActionResult> Login(LoginViewModel model, string returnUrl = null);

    }
}
