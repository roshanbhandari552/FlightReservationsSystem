using FlightReservationSystem.Models;

namespace FlightReservationSystem.Services.FlightServ
{
    public interface IflightService
    {
        Task<bool> AddAsync(Flight flight);

        Task<List<Flight>> GetAllAsync();

        Task DeleteAsync(Guid id);

        Task<Flight> GetByIdAsync(Guid id);

        Task<bool> UpdateAsync(Flight flight);
    }
}
