using FlightReservationSystem.Models;
using System.Linq.Expressions;

namespace FlightReservationSystem.Repositories.FlightRepo
{
    public interface IFlightRepository
    {
        Task<List<Flight>> GetAllAsync();

        Task<List<Flight>> GetAllWithDetailsAsync(); 

        Task<Flight> GetByIdAsync(Guid id);

        Task AddAsync(Flight flight);
        Task DeleteAsync(Flight flight);
        Task UpdateAsync(Flight flight);

        Task<bool> FlightExistsAsync(string flightNumber);

        IQueryable<Flight> GetFlightQuery();

    }
}
