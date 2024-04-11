﻿using RentalsWebApp.Data.Enums;
using RentalsWebApp.Models;

namespace RentalsWebApp.ViewModels
{
    public class BillingViewModel
    {
        public Months Month { get; set; }
        public string WaterAmount { get; set; }
        public string ElectricityAmount { get; set; }
        public string Statement { get; set; }

        public string UserId { get; set; }
        public List<BankAccount> BankAccount { get; set; }
    }
}
