using FlightReservationSystem.Attributes;
using FlightReservationSystem.Models;
using FlightReservationSystem.Repositories.FlightRepo;
using FlightReservationSystem.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace FlightReservationSystem.Services.FlightServ
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        public FlightService(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }
        public async Task<bool> AddAsync(FlightViewModel model)
        {
            if (model == null)
            {
                return false;
            }

            if (await _flightRepository.FlightExistsAsync(model.FlightNumber))
            {
                return false; // Indicate duplicate
            }

            var flight = new Flight
            {
                Id = Guid.NewGuid(),
                FlightNumber = model.FlightNumber,
                OriginAirportId = model.OriginAirportId,
                DestinationAirportId = model.DestinationAirportId,
                AircraftId = model.AircraftId,
                EstimatedDuration = model.EstimatedDuration,
                FlightDateTime = model.FlightDateTime
            };

            await _flightRepository.AddAsync(flight);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                return false;

            var flight = await _flightRepository.GetByIdAsync(id);
            if (flight == null)
                return false;

            await _flightRepository.DeleteAsync(flight);
            return true;
        }

        public async Task<List<Flight>> GetAllAsync()
        {
            return await _flightRepository.GetAllAsync();
        }

        public async Task<List<Flight>> GetAllWithDetailsAsync()
        {
            return await _flightRepository.GetAllWithDetailsAsync();
        }


        public async Task<Flight> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid flight ID.", nameof(id));
            }

            return await _flightRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateAsync(FlightViewModel model)
        {
            if (model == null)
            {
                return false;
            }
            var flight = await _flightRepository.GetByIdAsync(model.Id);

            if(flight == null)
            {
                return false;
            }

            flight.FlightNumber = model.FlightNumber;
            flight.OriginAirportId = model.OriginAirportId;
            flight.DestinationAirportId = model.DestinationAirportId;
            flight.AircraftId = model.AircraftId;
            flight.EstimatedDuration = model.EstimatedDuration;
            flight.FlightDateTime = model.FlightDateTime;

            await _flightRepository.UpdateAsync(flight);
            return true;
        }

      /*  public async Task<FlightViewModel?> GetEdit(Guid id)
        {
            var flight = await _flightRepository.GetByIdAsync(id);

            if (flight == null)
            {
                return null;
            }

            var model = new FlightViewModel
            {
                Id = flight.Id,
                FlightNumber = flight.FlightNumber,
                OriginAirportId = flight.OriginAirportId,
                DestinationAirportId = flight.DestinationAirportId,
                AircraftId = flight.AircraftId,
                EstimatedDuration = flight.EstimatedDuration,
                FlightDateTime = flight.FlightDateTime
            };
            return model;

        }*/

        public async Task<FlightViewModel?> GetEditAsync(Guid id)
        {
            if(id == Guid.Empty)
            {
                return null;
            }

            var flight = await _flightRepository.GetByIdAsync(id); 
            if (flight == null)
            {
                return null;
            }

            var model = new FlightViewModel
            {
                Id = flight.Id,
                FlightNumber = flight.FlightNumber,
                OriginAirportId = flight.OriginAirportId,
                DestinationAirportId = flight.DestinationAirportId,
                AircraftId = flight.AircraftId,
                EstimatedDuration = flight.EstimatedDuration,
                FlightDateTime = flight.FlightDateTime
            };
            return model;
        }

        public async Task<FlightSearchViewModel> FlightSearch(FlightSearchViewModel model)
        {
            var originId = Guid.Parse(model.SelectedOriginAirportId);
            var destinationId = Guid.Parse(model.SelectedDestinationAirportId);

            var flight = _flightRepository.GetFlightQuery();

            model.MatchingFlights = await flight
            .Where(f =>
                f.OriginAirportId == originId &&
                f.DestinationAirportId == destinationId &&
                f.FlightDateTime.Date == model.DepartureDate.Date
            ).ToListAsync();

            Console.WriteLine("IsRoundTrip: " + model.IsRoundTrip);
            // If round trip and return date is selected, get return flights
            if (model.IsRoundTrip && model.ReturnDate.HasValue)
            {
                model.ReturnFlights = await flight
                    .Where(f =>
                        f.OriginAirportId == destinationId &&
                        f.DestinationAirportId == originId &&
                        f.FlightDateTime.Date == model.ReturnDate.Value.Date)
                    .ToListAsync();
            }

            return model;
        }
    }
}