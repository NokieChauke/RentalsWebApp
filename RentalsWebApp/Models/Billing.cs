﻿using System.ComponentModel.DataAnnotations.Schema;

namespace RentalsWebApp.Models
{
    public class Billing
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public string WaterAmount { get; set; }
        public string ElectricityAmount { get; set; }
        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
