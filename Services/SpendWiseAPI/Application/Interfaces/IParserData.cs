using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IParserData
    {
        string GetUpdatedStringSpentOfCategoryWhenAddTransaction(string categoryBudgetPlan, string spentOfCategoryMonthlyPlan, string CurrentCategory, int amount);
    }
}
