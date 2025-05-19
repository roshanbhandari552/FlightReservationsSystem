using FlightReservationSystem.Helper;
using FlightReservationSystem.Models;
using FlightReservationSystem.Repositories.AircraftRepo;
using FlightReservationSystem.Services.AirportServ;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace FlightReservationSystem.Controllers
{
    public class AirportController : Controller
    {
        private readonly IAirportService airportService;

        public AirportController(IAirportService _airportService)
        {
           airportService = _airportService;
        }

        private List<string> GetCountryList()
        {
            return new List<string>
    {
        "United States",
        "Canada",
        "United Kingdom",
        "Germany",
        "France",
        "Nepal",
        "India",
        "Australia",
        "Japan",
        "Other"
    };
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var airport = await airportService.GetAllAsync();

            return View(airport);
        }

        // GET: Airport/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Countries = GetCountryList();
            return View();
        }

        // POST: Airport/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AirportViewModel model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    airportService.AddAsync(model);
                   
                    TempData["SuccessMessage"] = "Airport created successfully!";
                    return RedirectToAction("Index");
                    
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the airport.");
                    return View(model);
                }
            }

            ViewBag.Countries = GetCountryList(); // repopulate dropdown if needed
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {

            var airport = await airportService.GetByIdAsync(Id);
            if (airport == null)
            {
                return NotFound();
            }
            ViewBag.Countries = GetCountryList();
            var airportViewModel = new AirportViewModel
            {
                Id = airport.Id,
                Code = airport.Code,
                Name = airport.Name,
                City = airport.City,
                Country = airport.Country

            };
            return View(airportViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AirportViewModel model)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Countries = GetCountryList(); // repopulate dropdown if needed
                return View(model);
            }

            try
            {
               
                var success = await airportService.UpdateAsync(model);
                if(!success)
                {
                    return NotFound("Airport isn't found");
                }

                TempData["SuccessMessage"] = "Airport updated successfully!";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the airport.");
                ViewBag.Countries = GetCountryList(); // Repopulate dropdown
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await airportService.DeleteAsync(id);

                TempData["SuccessMessage"] = "Airport deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the airport.";
                return RedirectToAction("Index");
            }
        }


    }
}
