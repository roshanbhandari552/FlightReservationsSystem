using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
namespace FlightReservationSystem.Services.AirportServ
{
    public interface IAirportService
    {
        Task<bool> AddAsync(AirportViewModel model);
        Task<bool> UpdateAsync(AirportViewModel model);

        Task<bool> DeleteAsync(Guid id);

        Task<List<Airport>> GetAllAsync();

        Task<Airport> GetByIdAsync(Guid id);
    }
}
