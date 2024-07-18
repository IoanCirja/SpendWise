using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class MonthlyPlanGetNameDate
    {
        public Guid monthlyPlan_id { get; set; }
        public string plan_name { get; set; }
        public DateTime date { get; set; }
    }
}
