﻿using System.ComponentModel.DataAnnotations;

namespace FlightReservationSystem.Models
{
    public class Airport
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

       
        public string Name { get; set; }

        public string City { get; set; }

        public string Country { get; set; }
    }
}
