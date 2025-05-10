using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebApplication1.Models;

namespace FlightReservationSystem.Controllers
{
    public class FlightController : Controller
    {
        private readonly AppDbContext _context;

        public FlightController(AppDbContext context)
        {
            _context = context;
        }

        private void LoadDropdowns(FlightViewModel model)
        {
            model.Airports = _context.Airports
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Name} ({a.Code}) - {a.Country}"
                })
                .ToList();


            model.Aircrafts = _context.Aircrafts
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Model })
                .ToList();

            model.Aircrafts.Insert(0, new SelectListItem
            {
                Value = "",
                Text = "Select Aircraft",
                Disabled = false,
                Selected = true
            });
        }

        private void FlightLoadDropdowns(FlightSearchViewModel model)
        {
            var airport = _context.Airports
          .Select(a => new SelectListItem
          {
              Value = a.Id.ToString(),
              Text = $"{a.Name} ({a.Code}) - {a.Country}"
          })
          .ToList();

            model.DestinationAirports = airport;
            model.OriginAirports = new List<SelectListItem>(airport);
        }

        public IActionResult Index()
        {
            var flights = _context.Flights
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .Include(f => f.Aircraft)
                .ToList();

            return View(flights);
        }


        [HttpGet]
        public IActionResult Create()
        {
            
                var model = new FlightViewModel();
            LoadDropdowns(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlightViewModel model)
        {
            LoadDropdowns(model);

            if (_context.Flights.Any(f => f.FlightNumber == model.FlightNumber))
            {
                ModelState.AddModelError("FlightNumber", "Flight number already exists.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
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


            try
            {
                _context.Flights.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the flight.");
                return View(model);
            }
        

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
                var flight = await _context.Flights.FindAsync(id);
                if (flight == null) return NotFound();

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

                LoadDropdowns(model);
                return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FlightViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns(model);
                return View(model);
            }

            var flight = await _context.Flights.FindAsync(model.Id);
            if (flight == null) return NotFound();

            flight.FlightNumber = model.FlightNumber;
            flight.OriginAirportId = model.OriginAirportId;
            flight.DestinationAirportId = model.DestinationAirportId;
            flight.AircraftId = model.AircraftId;
            flight.EstimatedDuration = model.EstimatedDuration;
            flight.FlightDateTime = model.FlightDateTime;


            try
            {
                _context.Flights.Update(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the flight.");
                LoadDropdowns(model);
                return View(model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
                return NotFound();

            _context.Flights.Remove(flight);

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel { Message = "An error occurred while deleting the flight." });
            }
        }

        [HttpGet]
        public IActionResult FlightSearch()
        {
            var model = new FlightSearchViewModel();
            FlightLoadDropdowns(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult FLightSearch(FlightSearchViewModel model)
        {

            if (model.IsRoundTrip && !model.ReturnDate.HasValue)
            {
                ModelState.AddModelError("ReturnDate", "Return date is required for round trip.");
            }

          
            if (!ModelState.IsValid)
            {
                FlightLoadDropdowns(model);
                return View(model);
            }
            if (!model.IsRoundTrip)
            {
                // Clear ReturnDate errors if it is not selected
                ModelState.Remove(nameof(model.ReturnDate));
            }

           
            var originId = Guid.Parse(model.SelectedOriginAirportId);
            var destinationId = Guid.Parse(model.SelectedDestinationAirportId);

            model.MatchingFlights = _context.Flights
            .Include(f => f.OriginAirport)
            .Include(f => f.DestinationAirport)
            .Include(f => f.Aircraft)
            .Where(f =>
                f.OriginAirportId == originId &&
                f.DestinationAirportId ==destinationId&&
                f.FlightDateTime.Date == model.DepartureDate.Date
            ).ToList();

            Console.WriteLine("IsRoundTrip: " + model.IsRoundTrip);
            // If round trip and return date is selected, get return flights
            if (model.IsRoundTrip && model.ReturnDate.HasValue)
            {
                model.ReturnFlights = _context.Flights
                    .Include(f => f.OriginAirport)
                    .Include(f => f.DestinationAirport)
                    .Include(f => f.Aircraft)
                    .Where(f =>
                        f.OriginAirportId == destinationId &&
                        f.DestinationAirportId == originId &&
                        f.FlightDateTime.Date == model.ReturnDate.Value.Date)
                    .ToList();
            }
            TempData["Flights"] = JsonConvert.SerializeObject(model.MatchingFlights);
            TempData["IsRoundTrip"] = model.IsRoundTrip.ToString();
            TempData["ReturnFlights"] = JsonConvert.SerializeObject(model.ReturnFlights);
            return RedirectToAction("AvailableFlight");
        }

        [HttpGet]
        public IActionResult AvailableFlight()
        {
            if (TempData["Flights"] is not string outboundData)
                return RedirectToAction("FlightSearch");

            var outboundFlights = JsonConvert.DeserializeObject<List<Flight>>(outboundData);
            var returnFlights = TempData["ReturnFlights"] is string returnData
                ? JsonConvert.DeserializeObject<List<Flight>>(returnData)
                : new List<Flight>();

            bool isRoundTrip = TempData["IsRoundTrip"] is string roundTripFlag && bool.TryParse(roundTripFlag, out bool result) && result;

            var model = new FlightSearchResultViewModel
            {
                OutboundFlights = outboundFlights,
                ReturnFlights = returnFlights,
                IsRoundTrip = isRoundTrip
            };
            Console.WriteLine($"Return flights: {model.ReturnFlights.Count}");


            return View(model);
        }


    }
}
