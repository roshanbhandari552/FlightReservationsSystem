using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace FlightReservationSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> CreateBooking(Guid flightId)
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new BookingViewModel
            {
                UserId = user.Id,
                FlightId = flightId,

                Passengers = new List<PassengerViewModel>
        {
            new PassengerViewModel()  // one empty passenger to show
        }
            };

            return View(model);  
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBooking(BookingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Reload the form with validation messages
                return View(model);
            }
            var flight = await _context.Flights.FindAsync(model.FlightId);
            if (flight == null)
            {
                ModelState.AddModelError("", "The selected flight does not exist.");
                return View(model);
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                // Generate a unique Booking ID
                var bookingId = Guid.NewGuid();

                var booking = new Booking
                {
                    Id = bookingId,
                    FlightId = model.FlightId,
                    UserId = model.UserId,
                    Email = model.Email,
                    PaymentMethod = model.PaymentMethod,
                    BookingDate = DateTime.UtcNow,
                    Passengers = model.Passengers.Select(p => new Passenger
                    {
                        Id = Guid.NewGuid(),
                        FullName = p.FullName,
                        Gender = p.Gender,
                        DateOfBirth = p.DateOfBirth,
                        PassportNumber = p.PassportNumber,
                        BookingId = bookingId
                    }).ToList()
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction("ConfirmationBooking", new { id = booking.Id });
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                return Content($"Error: {innerMessage}");
                /*Console.WriteLine($"Booking creation failed: {ex.Message}");

                ModelState.AddModelError(string.Empty, "An error occurred while processing your booking. Please try again.");

                return View(model);*/
            }
        }

        public async Task<IActionResult> ConfirmationBooking(Guid id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Flight)
        .ThenInclude(f => f.OriginAirport)
    .Include(b => b.Flight)
        .ThenInclude(f => f.DestinationAirport)

                .Include(b => b.Passengers)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return View(booking);
        }

        public async Task<IActionResult> MyBooking()
        {
            var user = await _userManager.GetUserAsync(User);

            var bookings = await _context.Bookings
                .Include(b => b.Flight)
                .ThenInclude(f => f.OriginAirport)
                .Include(b => b.Flight.DestinationAirport)
                 .Include(b => b.Passengers)
                .Where(b => b.UserId == user.Id)
                .ToListAsync();

            return View(bookings);
        }

        public async Task<IActionResult> AllBooking()
        {
            var user = await _userManager.GetUserAsync(User);

            var bookings = await _context.Bookings
                .Include(b => b.Flight)
                .ThenInclude(f => f.OriginAirport)
                .Include(b => b.Flight.DestinationAirport)
                 .Include(b => b.Passengers)
                .ToListAsync();

            return View(bookings);
        }


    }
}
