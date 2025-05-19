using Microsoft.IdentityModel.Tokens;
using FlightReservationSystem.Models;
using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightReservationSystem.Repositories.AircraftRepo
{
    public class AircraftRepository : IAircraftRepository
    {
        private readonly AppDbContext _context;

        public AircraftRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Aircraft aircraft)
        {
            _context.Aircrafts.Add(aircraft);
            await _context.SaveChangesAsync();
        }



        public async Task DeleteAsync(Guid id)
        {
            var aircraft = await _context.Aircrafts.FindAsync(id);
            if (aircraft != null)
            {
                _context.Aircrafts.Remove(aircraft);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Aircraft>> GetAllAsync()
        {
            return await _context.Aircrafts.ToListAsync();
           
        }

        public async Task<Aircraft> GetByIdAsync(Guid id)
        {
            return await _context.Aircrafts.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Aircraft aircraft)
        {
            _context.Aircrafts.Update(aircraft);
            await _context.SaveChangesAsync();

        }
    }
 
    
}
