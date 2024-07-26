using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Statistics
    {
        public string mostUsedPlanName {  get; set; }
        public string biggestTransactionNameForever { get; set; }
        public double biggestTransactionAmountForever {  get; set; }
        public string smallestTransactionNameForever { get; set; }
        public double smallestTransactionAmountForever { get; set; }
        public string biggestTransactionNameCurrentPlan { get; set; }
        public double biggestTransactionAmountCurrentPlan { get; set; }
        public string smallestTransactionNameCurrentPlan { get; set; }  
        public double smallestTransactionAmountCurrentPlan { get; set; }
        public string firstCategoryFull { get; set; }
        public double TotalMoney { get; set; }
        public double SpendMoney { get; set; }

    }
}
