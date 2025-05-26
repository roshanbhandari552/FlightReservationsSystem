using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;

namespace FlightReservationSystem.Services.FlightServ
{
    public interface IFlightService
    {
        Task<bool> AddAsync(FlightViewModel moel);

        Task<List<Flight>> GetAllAsync();

        Task<List<Flight>> GetAllWithDetailsAsync();

        Task<bool> DeleteAsync(Guid id);

        Task<Flight> GetByIdAsync(Guid id);

        Task<bool> UpdateAsync(FlightViewModel model);

        Task<FlightViewModel?> GetEditAsync(Guid id);

        Task<FlightSearchViewModel> FlightSearch(FlightSearchViewModel model);
    }
}
