using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class BudgetPlanGetPopular
    {
        public Guid plan_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        public int noCategory { get; set; }

        public string category { get; set; }
        public string image { get; set; }
        public int numberOfUse { get; set; }
    }
}
