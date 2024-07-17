using Application.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MonthlyPlanRepository : IMonthlyPlanRepository
    {
        private readonly IDatabaseContext _databaseContext;
        public MonthlyPlanRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public async Task<bool> AddMonthlyPlans(MonthlyPlan monthlyPlan)
        {
            var query = "INSERT INTO [SpendWise].[MonthlyPlan] ([monthlyPlan_id], [user_id], [plan_id], [date], [totalAmount], [amountSpent], [priceByCategory], [spentOfCategory]) VALUES (NEWID(), @UserID, @PlanID, @Date, @TotalAmount, @AmountSpent, @PriceByCategory, @SpentOfCategory)";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", monthlyPlan.user_id, DbType.Guid);
            parameters.Add("PlanID", monthlyPlan.plan_id, DbType.Guid);
            parameters.Add("Date", monthlyPlan.date, DbType.Date);
            parameters.Add("TotalAmount", monthlyPlan.totalAmount, DbType.Int64);
            parameters.Add("AmountSpent", monthlyPlan.amountSpent, DbType.Int64);
            parameters.Add("PriceByCategory", monthlyPlan.priceByCategory, DbType.String);
            parameters.Add("SpentOfCategory", monthlyPlan.spentOfCategory, DbType.String);


            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }

        public async Task<bool> CancelMonthlyPlan(Guid id)
        {
            var query = "UPDATE [SpendWise].[MonthlyPlan]  SET [status]='Canceled' where [monthlyPlan_id]=@MonthlyPlanID";
            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, new { MonthlyPlanID = id });
            return result != 0;
        }

    }
}
