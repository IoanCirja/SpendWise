﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts
{
    public class TransactionsContract
    {
        public string name { get; set; }
        public Guid monthlyPlan_id { get; set; }
        public DateTime date { get; set; }
        public string category { get; set; }
        public double amount { get; set; }
    }
}