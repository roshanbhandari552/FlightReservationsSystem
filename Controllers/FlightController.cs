using FlightReservationSystem.Models;
using FlightReservationSystem.Services.DropDownServices;
using FlightReservationSystem.Services.FlightServ;
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
        private readonly IFlightService _flightService;
        private readonly IDropDownService _dropDownService;

        public FlightController(IFlightService flightService, IDropDownService dropDownService)
        {
            _flightService = flightService;
            _dropDownService = dropDownService;
        }

        private async Task LoadDropdowns(FlightViewModel model)
        {
           model.Airports =await  _dropDownService.GetAirportDropDownAsync();
           model.Aircrafts = await _dropDownService.GetAircraftDropDownAsync();
        }

        private async Task FlightLoadDropdowns(FlightSearchViewModel model)
        {
           await _dropDownService.GetAllAirportDropDownAsync(model);
        }

        public async Task<IActionResult> Index()
        {
            var flights = await _flightService.GetAllWithDetailsAsync();

            return View(flights);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new FlightViewModel();
            await LoadDropdowns(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlightViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadDropdowns(model);
                return View(model);
            }  

            try
            {
               var success = await _flightService.AddAsync(model);
                if (!success)
                {
                    ModelState.AddModelError("FlightNumber", "Flight number already exists.");
                    return View(model);
                }
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
               var flightViewModel =  await _flightService.GetEditAsync(id);
            if(flightViewModel == null)
            {
                return NotFound();
            }

               await LoadDropdowns(flightViewModel);
               return View(flightViewModel);
        }

       [HttpPost]
        public async Task<IActionResult> Edit(FlightViewModel model)
        {
            if (!ModelState.IsValid)
            {
               await LoadDropdowns(model);
                return View(model);
            }

            try
            {
               await _flightService.UpdateAsync(model);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the flight.");
                await LoadDropdowns(model);
                return View(model);
            }
        }


        [HttpPost]
         public async Task<IActionResult> Delete(Guid id)
         {
             if (id == Guid.Empty)
                 return NotFound();

             try
             {
                 var success = await _flightService.DeleteAsync(id);
                 if (!success)
                 {
                     return NotFound(); 
                 }

                 TempData["SuccessMessage"] = "Flight deleted successfully!";
                 return RedirectToAction("Index");
             }
             catch (Exception)
             {
                 return View("Error", new ErrorViewModel
                 {
                     Message = "An error occurred while deleting the flight."
                 });
             }
         }

         [HttpGet]
        public async Task<IActionResult> FlightSearch()
        {
            var model = new FlightSearchViewModel();
            await FlightLoadDropdowns(model);
            return View(model);
        }

          [HttpPost]
            public async Task<IActionResult> FLightSearch(FlightSearchViewModel model)
            {

                if (model.IsRoundTrip && !model.ReturnDate.HasValue)
                {
                    ModelState.AddModelError("ReturnDate", "Return date is required for round trip.");
                }

                if (!ModelState.IsValid)
                {
                    await FlightLoadDropdowns(model);
                    return View(model);
                }
                if (!model.IsRoundTrip)
                {
                    // Clear ReturnDate errors if it is not selected
                    ModelState.Remove(nameof(model.ReturnDate));
                }

                var updatedModel = await _flightService.FlightSearch(model);
                TempData["Flights"] = JsonConvert.SerializeObject(updatedModel.MatchingFlights);
                TempData["IsRoundTrip"] = updatedModel.IsRoundTrip.ToString();
                TempData["ReturnFlights"] = JsonConvert.SerializeObject(updatedModel.ReturnFlights);
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
