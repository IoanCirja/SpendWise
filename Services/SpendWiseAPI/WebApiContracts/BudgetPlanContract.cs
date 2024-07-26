using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts
{
    public class BudgetPlanContract
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NoCategory { get; set; }
        public string Category { get; set; }
        public string Imagine { get; set; }
        public Guid user_id { get; set; }
    }
}
