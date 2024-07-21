using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts
{
    public class BudgetPlanEditContract
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NoCategory { get; set; }
        public string Category { get; set; }
        public string Image { get; set; }
    }
}
