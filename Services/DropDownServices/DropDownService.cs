using FlightReservationSystem.Repositories.AircraftRepo;
using FlightReservationSystem.Repositories.AirportRwpo;
using FlightReservationSystem.Repositories.FlightRepo;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FlightReservationSystem.Services.DropDownServices
{
    public class DropDownService : IDropDownService
    {
        private readonly IAirportRepository _airportRepository;
        private readonly IAircraftRepository _aircraftRepository;
        public DropDownService(IAirportRepository airportRepository, IAircraftRepository aircraftRepository) { 
            _airportRepository = airportRepository;
            _aircraftRepository = aircraftRepository;
        }

        public async Task<List<SelectListItem>> GetAirportDropDownAsync()
        {
           var airportDropdown =  await _airportRepository.GetAll();
            return airportDropdown.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.Name} ({a.Code}) - {a.Country}"
            }).ToList();
        }

        public async Task<List<SelectListItem>> GetAircraftDropDownAsync()
        {
            var aircrafts = await _aircraftRepository.GetAllAsync();

            return aircrafts.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Model
            }).ToList();

            /* if (includePlaceholder)
             {
                 items.Insert(0, new SelectListItem
                 {
                     Value = "",
                     Text = "Select Aircraft",
                     Disabled = false,
                     Selected = true
                 });
             }

             return items;*/
        }

        public async Task GetAllAirportDropDownAsync(FlightSearchViewModel model)
        {
            var airport = await _airportRepository.GetAll();
            var airport1 = airport.Select(a => new SelectListItem
          {
              Value = a.Id.ToString(),
              Text = $"{a.Name} ({a.Code}) - {a.Country}"
          })
          .ToList();

            model.DestinationAirports = airport1;
            model.OriginAirports = new List<SelectListItem>(airport1);
        }


    }
}
