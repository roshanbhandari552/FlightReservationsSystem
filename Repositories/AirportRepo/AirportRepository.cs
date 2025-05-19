
using FlightReservationSystem.Models;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace FlightReservationSystem.Repositories.AirportRwpo
{
    public class AirportRepository : IAirportRepository
    {
        private readonly AppDbContext _appDbContext;
        public AirportRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task AddAsync(Airport airport)
        {
            _appDbContext.Airports.Add(airport);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var airport = await _appDbContext.Airports.FirstOrDefaultAsync(x => x.Id == id);
            _appDbContext.Airports.Remove(airport);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<Airport>> GetAll()
        {
           return await _appDbContext.Airports.ToListAsync();
        }

        public async Task<Airport> GetById(Guid id)
        {
            return await _appDbContext.Airports.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Airport airport)
        {
            _appDbContext.Airports.Update(airport);
            await _appDbContext.SaveChangesAsync();

        }
    }
}
