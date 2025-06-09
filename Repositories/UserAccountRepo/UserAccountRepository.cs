using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace FlightReservationSystem.Repositories.UserAccountRepo
{
    public class UserAccountRepository : IUserAccountRepository
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public UserAccountRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users.ToList();
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
       => await _userManager.UpdateAsync(user);

        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        => await _userManager.DeleteAsync(user);

        public async Task<List<ApplicationUser>> SearchUsersAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return await _userManager.Users.ToListAsync();

            return await _userManager.Users
                .Where(u => u.UserName.Contains(query) || u.Email.Contains(query))
                .ToListAsync();
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> AddLoginAsync(ApplicationUser user, UserLoginInfo info)
        {
            return await _userManager.AddLoginAsync(user, info);
        }


    }
}
