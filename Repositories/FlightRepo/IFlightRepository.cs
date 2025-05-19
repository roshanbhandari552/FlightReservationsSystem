using FlightReservationSystem.Models;

namespace FlightReservationSystem.Repositories.FlightRepo
{
    public interface IFlightRepository
    {
        Task<List<Flight>> GetAllAsync();

        Task<Flight> GetByIdAsync(Guid id);

        Task AddAsync(Flight flight);
        Task DeleteAsync(Flight flight);
        Task UpdateAsync(Flight flight);

    }
}
