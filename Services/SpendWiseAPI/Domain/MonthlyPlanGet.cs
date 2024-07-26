using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MonthlyPlanGet
    {
        public Guid monthlyPlan_id { get; set; }
        public Guid user_id { get; set; }
        public Guid plan_id { get; set; }
        public string plan_name { get; set; }
        public string description { get; set; }
        public int noCategory { get; set; }
        public string category { get; set; }
        public string image {  get; set; }
        public DateTime date { get; set; }
        public double totalAmount { get; set; }
        public double amountSpent { get; set; }
        public string status { get; set; }
        public string priceByCategory { get; set; }
        public string spentOfCategory { get; set; }
    }
}
