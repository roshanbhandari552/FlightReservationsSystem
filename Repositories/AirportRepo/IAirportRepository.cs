using FlightReservationSystem.Models;

namespace FlightReservationSystem.Repositories.AirportRwpo
{
    public interface IAirportRepository
    {
        Task<List<Airport>> GetAll();
        Task<Airport> GetById(Guid id);
        Task AddAsync(Airport airport);

        Task UpdateAsync(Airport airport);

        Task DeleteAsync(Guid id);

    }
}
