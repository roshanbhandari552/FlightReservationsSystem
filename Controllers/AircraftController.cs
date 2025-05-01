using FlightReservationSystem.Helper;
using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace FlightReservationSystem.Controllers
{
    public class AircraftController : Controller
    {

        private readonly AppDbContext _context;

        public AircraftController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var aircraft = _context.Aircrafts.ToList(); ;

            return View(aircraft);
        }

        // GET: Aircraft/Create
        [HttpGet]
        public IActionResult Create()
        {
          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AircraftViewModel mode)
        {
            if (ModelState.IsValid)
            {
                var aircraft = new Aircraft
                {
                    Id = Guid.NewGuid(),
                    Model = TextCapitalize.CapitalizeEachWord(mode.Model),
                    Capacity = mode.Capacity,
                    Manufacturer = TextCapitalize.CapitalizeEachWord(mode.Manufacturer),
                };

                try
                {
                    _context.Aircrafts.Add(aircraft);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Aircraft created successfully!";
                    return RedirectToAction("Index", "Aircraft");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the airport.");
                    return View(mode);
                }
            }

            return View(mode);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {

            var aircraft = await _context.Aircrafts.FindAsync(Id);
            if (aircraft == null)
            {
                return NotFound();
            }

            var aircraftViewModel = new AircraftViewModel
            {
                Id = aircraft.Id,
                Model = aircraft.Model,
                Capacity = aircraft.Capacity,
                Manufacturer = aircraft.Manufacturer,

            };
            return View(aircraftViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AircraftViewModel mode)
        {

            if (!ModelState.IsValid)
            {
                return View(mode);
            }

            var aircraft = await _context.Aircrafts.FindAsync(mode.Id);
            if (aircraft == null)
            {
                return NotFound();
            }

            try
            {
                // Update the properties
                aircraft.Model = mode.Model;
                aircraft.Capacity = mode.Capacity;
                aircraft.Manufacturer = mode.Manufacturer;
                
                _context.Aircrafts.Update(aircraft);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Aircraft updated successfully!";
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the aircraft.");
                return View(mode);
            }

            return View(aircraft);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var aircraft = await _context.Aircrafts.FindAsync(id);

            if (aircraft == null)
            {
                return NotFound();
            }

            try
            {
                _context.Aircrafts.Remove(aircraft);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Aircraft deleted successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the aircraft.";
                return RedirectToAction("Index");
            }
        }

    }
}
