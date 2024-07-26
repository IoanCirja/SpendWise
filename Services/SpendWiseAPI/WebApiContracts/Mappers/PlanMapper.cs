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
        public static BudgetPlan MapTestToDomain(this BudgetPlanContract budgetPlanContract)
        {
            return new BudgetPlan
            {
                name = budgetPlanContract.Name,
                description = budgetPlanContract.Description,
                category = budgetPlanContract.Category,
                noCategory = budgetPlanContract.NoCategory,
                created_by = budgetPlanContract.user_id,
                image = budgetPlanContract.Imagine,
                creationDate = budgetPlanContract.creationDate,
            };
        }
        public static BudgetPlan MapPlanEditToDomain(this BudgetPlanEditContract budgetPlanContract)
        {
            return new BudgetPlan
            {
                name = budgetPlanContract.Name,
                description = budgetPlanContract.Description,
                category = budgetPlanContract.Category,
                noCategory = budgetPlanContract.NoCategory,
                image = budgetPlanContract.Image
            };
        }
    }
}
