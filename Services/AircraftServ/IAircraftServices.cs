using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;

namespace FlightReservationSystem.Services.AircraftServ
{
    public interface IAircraftServices
    {
        Task<List<Aircraft>> GetAllAsync();
        Task<Aircraft> GetByIdAsync(Guid id);
        Task AddAsync(AircraftViewModel mode);
        Task<bool> UpdateAsync(AircraftViewModel mode);
        Task DeleteAsync(Guid id);
    }
}
