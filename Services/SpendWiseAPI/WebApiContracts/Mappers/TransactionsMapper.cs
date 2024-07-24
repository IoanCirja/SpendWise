using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiContracts.Mappers
{
    public static class TransactionsMapper
    {
        public static Transactions MapTestToDomain(this TransactionsContract transactions)
        {
            return new Transactions
            {
                name = transactions.name,
                monthlyPlan_id = transactions.monthlyPlan_id,
                date = transactions.date,
                category = transactions.category,
                amount = transactions.amount,
            };
        }
    }
}
