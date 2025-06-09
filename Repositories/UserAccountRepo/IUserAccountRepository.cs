using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace FlightReservationSystem.Repositories.UserAccountRepo
{
    public interface IUserAccountRepository
    {
        Task<ApplicationUser?> GetUserByIdAsync(string id);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        List<ApplicationUser> GetAllUsers();

        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);

        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);
        Task<IdentityResult> DeleteUserAsync(ApplicationUser user);
        Task<List<ApplicationUser>> SearchUsersAsync(string query);

        Task<IdentityResult> AddLoginAsync(ApplicationUser user, UserLoginInfo info);


    }
}
