using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts.Mappers
{
    public static class PlanMapper
    {
        public static BudgetPlan MapTestToDomain(this BudgetPlanContract budgetPlanContract,Guid user_id)
        {
            return new BudgetPlan
            {
                Name = budgetPlanContract.Name,
                Description = budgetPlanContract.Description,
                Category = budgetPlanContract.Category,
                NoCategory = budgetPlanContract.NoCategory,
                CreatedBy = user_id,
                Imagine = budgetPlanContract.Imagine
            };
        }
    }
}
