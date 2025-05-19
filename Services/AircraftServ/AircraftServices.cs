using FlightReservationSystem.Helper;
using FlightReservationSystem.Models;
using FlightReservationSystem.Repositories.AircraftRepo;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FlightReservationSystem.Services.AircraftServ
{
    public class AircraftServices : IAircraftServices
    {
        private readonly IAircraftRepository _aircraftRepository;
        public AircraftServices(IAircraftRepository aircraftRepository)
        {
            _aircraftRepository = aircraftRepository;
        }

        public async Task AddAsync(AircraftViewModel mode)
        {

            var aircraft = new Aircraft
            {
                Id = Guid.NewGuid(),
                Model = TextCapitalize.CapitalizeEachWord(mode.Model),
                Capacity = mode.Capacity,
                Manufacturer = TextCapitalize.CapitalizeEachWord(mode.Manufacturer)

            };
            await _aircraftRepository.AddAsync(aircraft);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _aircraftRepository.DeleteAsync(id);
        }

        public async Task<List<Aircraft>> GetAllAsync()
        {
            return await _aircraftRepository.GetAllAsync();
        }

        public async Task<Aircraft> GetByIdAsync(Guid id)
        {
            return await _aircraftRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(AircraftViewModel mode)
        {
            var aircraft = await _aircraftRepository.GetByIdAsync(mode.Id);

            if (aircraft == null)
            {
                return false;
            }

            // Update the properties
            aircraft.Model = TextCapitalize.CapitalizeEachWord(mode.Model);
            aircraft.Capacity = mode.Capacity;
            aircraft.Manufacturer = TextCapitalize.CapitalizeEachWord(mode.Manufacturer);

            await _aircraftRepository.UpdateAsync(aircraft);
            return true;
        }
    }
}
