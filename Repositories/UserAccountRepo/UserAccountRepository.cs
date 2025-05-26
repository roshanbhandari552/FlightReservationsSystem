using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace FlightReservationSystem.Repositories.UserAccountRepo
{
    public class UserAccountRepository : IUserAccountRepository
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserAccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task<IActionResult> DeleteUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> EditUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> EditUser(EditUserViewModel model, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> IsEmailAvailable(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            throw new NotImplementedException();
        }

        public async Task Register(Employee model)
        {
            var user = new ApplicationUser { FirstName = model.FirstName, Email = model.Email, UserName = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
        }

        public Task<IActionResult> SearchUsers(string query)
        {
            throw new NotImplementedException();
        }
    }
}
