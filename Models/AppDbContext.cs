
using FlightReservationSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

       

        public DbSet<Airport> Airports { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Payment> Payments { get; set; }



        // Seed sample data (optional)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.OriginAirport)
                .WithMany()
                .HasForeignKey(f => f.OriginAirportId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevents cascade

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DestinationAirport)
                .WithMany()
                .HasForeignKey(f => f.DestinationAirportId)
                .OnDelete(DeleteBehavior.Restrict);  // Prevents cascade
        


    }

    }
}
