using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        
        public IActionResult GetEmployee()
        {
            var employees = _context.Users.ToList();
            Console.WriteLine("Users count: " + employees.Count);
            return View(employees);
        }
    }
}
