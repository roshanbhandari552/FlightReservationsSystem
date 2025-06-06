﻿using FlightReservationSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using WebApplication1.Models;

namespace FlightReservationSystem.Repositories.FlightRepo
{
    public class FlightRepository : IFlightRepository
    {
        private readonly AppDbContext _context;
        public FlightRepository(AppDbContext context) {
            _context = context;
        }

        public async Task AddAsync(Flight flight)
        {
           _context.Flights.Add(flight);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Flight flight)
        {
           _context.Flights.Remove(flight);
           await _context.SaveChangesAsync();
        }

        public async Task<List<Flight>> GetAllAsync()
        {
           return await _context.Flights.ToListAsync();
        }

        public async Task<List<Flight>> GetAllWithDetailsAsync()
        {
            return await _context.Flights
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .Include(f => f.Aircraft)
                .ToListAsync();
        }

        public async Task<Flight> GetByIdAsync(Guid id)
        {
            return await _context.Flights.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task UpdateAsync(Flight flight)
        {
            _context.Flights.Update(flight);
            await _context.SaveChangesAsync();
            
        }

        //To check the whether the flight number is unique or not
        public async Task<bool> FlightExistsAsync(string flightNumber)
        {
            return await _context.Flights.AnyAsync(f => f.FlightNumber == flightNumber);
        }

        public IQueryable<Flight> GetFlightQuery()
        {
            return _context.Flights
                .Include(f => f.OriginAirport)
                .Include(f => f.DestinationAirport)
                .Include(f => f.Aircraft);
        }

    }
}
