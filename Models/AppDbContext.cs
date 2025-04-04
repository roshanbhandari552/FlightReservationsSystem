
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

        public DbSet<Employee> Employees { get; set; }
       

        // Seed sample data (optional)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().HasData(
             new Employee
             {
                 Id = 1,
                 FirstName ="Roshan",
                 Email = "roshan@example.com",
                 Password = "Test1234",           // Note: For demo only — never store raw passwords!
                 ConfirmPassword = "Test1234"
             }
            );
        }
    }
}
