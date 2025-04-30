using FlightReservationSystem.Helper;
using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace FlightReservationSystem.Controllers
{
    public class AirportController : Controller
    {
        private readonly AppDbContext _context;

        public AirportController(AppDbContext context)
        {
            _context = context;
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
        public IActionResult Index()
        {
            var airport = _context.Airports;

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
                var airport = new Airport
                {
                    Id = Guid.NewGuid(),
                    Name = TextCapitalize.CapitalizeEachWord(model.Name),
                    City = TextCapitalize.CapitalizeEachWord(model.City),
                    Country = TextCapitalize.CapitalizeEachWord(model.Country),
                    Code = TextCapitalize.CapitalizeEachWord(model.Code)
                };

                try
                {
                    _context.Airports.Add(airport);
                    await _context.SaveChangesAsync();

                   
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

            var airport = await _context.Airports.FindAsync(Id);
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

            var airport = await _context.Airports.FindAsync(model.Id);
            if (airport == null)
            {
                return NotFound();
            }

            try
            {
                // Update the properties
                airport.Code = model.Code;
                airport.Name = model.Name;
                airport.City = model.City;
                airport.Country = model.Country;
                _context.Airports.Update(airport);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Airport updated successfully!";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the airport.");
                ViewBag.Countries = GetCountryList(); // Repopulate dropdown
                return View(model);
            }

            return View(airport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var airport = await _context.Airports.FindAsync(id);

            if (airport == null)
            {
                return NotFound();
            }

            try
            {
                _context.Airports.Remove(airport);
                await _context.SaveChangesAsync();

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
