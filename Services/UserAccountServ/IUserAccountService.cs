using FlightReservationSystem.ViewModel;

namespace FlightReservationSystem.Services.UserAccountServ
{
    public interface IUserAccountService
    {
        Task<(bool IsSuccessful, IEnumerable<string> Errors)> RegisterUserAsync(RegisterViewModel model);
    }
}
