using FlightReservationSystem.Models;

namespace FlightReservationSystem.Repositories.AircraftRepo
{
    public interface IAircraftRepository
    {
        Task<List<Aircraft>> GetAllAsync();
        Task<Aircraft> GetByIdAsync(Guid id);
        Task AddAsync(Aircraft aircraft);
        Task UpdateAsync(Aircraft aircraft);
        Task DeleteAsync(Guid id);
    }
}
