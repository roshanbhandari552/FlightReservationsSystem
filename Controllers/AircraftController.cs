using FlightReservationSystem.Helper;
using FlightReservationSystem.Models;
using FlightReservationSystem.Services.AircraftServ;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace FlightReservationSystem.Controllers
{
    public class AircraftController : Controller
    {

        /*private readonly AppDbContext _context;*/

        /* public AircraftController(AppDbContext context)
         {
             _context = context;
         }*/

        private readonly IAircraftServices  _aircraftServices;
        public AircraftController(IAircraftServices aircraftServices)
        {
            _aircraftServices = aircraftServices;
        }

        /*  [HttpGet]
          public IActionResult Index()
          {
              var aircraft = _context.Aircrafts.ToList(); ;

              return View(aircraft);
          }*/

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var aircraft = await _aircraftServices.GetAllAsync();

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
            if (!ModelState.IsValid)
            {
                return View(mode);
            }

                try
                { 
                   await _aircraftServices.AddAsync(mode);

                    TempData["SuccessMessage"] = "Aircraft created successfully!";
                    return RedirectToAction("Index", "Aircraft");

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the airport.");
                    return View(mode);
                }
            
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var aircraft = await _aircraftServices.GetByIdAsync(Id);
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

            try 
            { 
            var success = await _aircraftServices.UpdateAsync(mode);
      
                if (!success)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = "Aircraft updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the aircraft.");
                return View(mode);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
           /* var aircraft = await _context.Aircrafts.FindAsync(id);*/
            var aircraft = await _aircraftServices.GetByIdAsync(id);

            if (aircraft == null)
            {
                return NotFound();
            }

            try
            {
              /*  _context.Aircrafts.Remove(aircraft);
                await _context.SaveChangesAsync();*/
             await  _aircraftServices.DeleteAsync(id);

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
