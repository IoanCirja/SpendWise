using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts
{
    public class MonthlyPlanContract
    {
        public Guid user_id { get; set; }
        public Guid plan_id { get; set; }
        public DateTime date { get; set; }
        public double totalAmount { get; set; }
        public double amountSpent { get; set; }
        public string priceByCategory { get; set; }
        public string spentOfCategory { get; set; }
    }
}
