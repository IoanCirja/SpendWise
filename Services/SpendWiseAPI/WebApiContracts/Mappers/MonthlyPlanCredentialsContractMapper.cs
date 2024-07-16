using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace WebApiContracts.Mappers
{
    public static class MonthlyPlanCredentialsContractMapper
    {
        public static MonthlyPlan MapTestToDomain(this MonthlyPlanContract monthlyplan)
        {
            return new MonthlyPlan
            {
                user_id = monthlyplan.user_id,
                plan_id = monthlyplan.plan_id,
                date = monthlyplan.date,
                totalAmount = monthlyplan.totalAmount,
                amountSpent = monthlyplan.amountSpent,
                priceByCategory = monthlyplan.priceByCategory,
                spentOfCategory = monthlyplan.spentOfCategory,
            };
        }
    }
}
