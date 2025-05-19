using FlightReservationSystem.Helper;
using FlightReservationSystem.Models;
using FlightReservationSystem.Repositories.AirportRwpo;
using FlightReservationSystem.ViewModel;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace FlightReservationSystem.Services.AirportServ
{
    public class AirportService : IAirportService
    {
        private readonly IAirportRepository _airportRepository;
        public AirportService(IAirportRepository airportRepository)
        {

            _airportRepository = airportRepository;
        }
        public async Task<bool> AddAsync(AirportViewModel model)
        {
            
            var airport = new Airport
            {
                Id = Guid.NewGuid(),
                Name = TextCapitalize.CapitalizeEachWord(model.Name),
                City = TextCapitalize.CapitalizeEachWord(model.City),
                Country = TextCapitalize.CapitalizeEachWord(model.Country),
                Code = TextCapitalize.CapitalizeEachWord(model.Code)
            };
         
            await _airportRepository.AddAsync(airport);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var airport = await _airportRepository.GetById(id);

            if (airport == null)
            {
                return false;
            }
            await _airportRepository.DeleteAsync(id);
            return true;


        }

        public async Task<List<Airport>> GetAllAsync()
        {
           return await _airportRepository.GetAll();
        }

        public async Task<Airport> GetByIdAsync(Guid id)
        {
            if(id == null)
            {
                return null;
            }
            var airport = await _airportRepository.GetById(id);
            return airport;
        }

        public async Task<bool> UpdateAsync(AirportViewModel model)
        {
            var airport = await GetByIdAsync(model.Id);
            if(airport == null)
            {
                return false;
            }

            // Update the properties
            airport.Code = model.Code;
            airport.Name = model.Name;
            airport.City = model.City;
            airport.Country = model.Country;

            await _airportRepository.UpdateAsync(airport);
            return true;

        }
    }
}
