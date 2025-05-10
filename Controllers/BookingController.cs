using FlightReservationSystem.Models;
using FlightReservationSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace FlightReservationSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly AppDbContext _context;

        public BookingController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult CreateBooking(Guid flightId)
        {
            var model = new BookingViewModel
            {
                UserId = "fdde5d5c-f408-409d-9f36-bab8e5135f0a",
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

            try
            {
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


    }
}
