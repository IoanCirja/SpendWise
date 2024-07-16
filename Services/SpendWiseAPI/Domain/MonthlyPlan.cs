using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain
{
    public class MonthlyPlan
    {
        public Guid monthlyPlan_id { get; set; }
        public Guid user_id { get; set; }
        public Guid plan_id { get; set; }
        public DateOnly date { get; set; }
        public int totalAmount { get; set; }
        public int amountSpent { get; set; }
        public string priceByCategory { get; set; }
        public string spentOfCategory { get; set; }
    }

}
