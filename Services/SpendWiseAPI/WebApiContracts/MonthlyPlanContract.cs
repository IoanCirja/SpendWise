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
        public int totalAmount { get; set; }
        public int amountSpent { get; set; }
        public string priceByCategory { get; set; }
        public string spentOfCategory { get; set; }
    }
}
