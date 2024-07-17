using Application.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BudgetPlanRepository : IBudgetPlanRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public BudgetPlanRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public List<BudgetPlanGet> GetPlans()
        {
            var sql = "select [pd].[plan_id], [pd].[name], [pd].[description], [pd].[noCategory], [pd].[category], [pd].[image], [us].[name] as 'created_by'  from [SpendWise].[PlanDetails] pd, [SpendWise].[Users] us where [pd].[created_by]=[us].[user_id]";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<BudgetPlanGet>(sql).ToList();
            return file;
        }
        public List<BudgetPlanGet> GetPlan(Guid id)
        {
            var sql = "select [pd].[plan_id], [pd].[name], [pd].[description], [pd].[noCategory], [pd].[category], [pd].[image], [us].[name] as 'created_by'  from [SpendWise].[PlanDetails] pd, [SpendWise].[Users] us where [pd].[created_by]=[us].[user_id] and [plan_id]=@PlanID";
            var connection = _databaseContext.GetDbConnection();
            var plan = connection.Query<BudgetPlanGet>(sql, new { PlanID = id }).ToList();
            return plan;
        }
        public List<BudgetPlanGetPopular> GetPopularFivePlans()
        {
            var sql = "select top(5) [pd].[plan_id], [pd].[name], [pd].[description], [pd].[noCategory], [pd].[category], [pd].[image], (select count([mp].[plan_id]) from [SpendWise].[MonthlyPlan] mp where [mp].[plan_id] = [pd].[plan_id] group by [mp].[plan_id]) as 'numberOfUse'  from [SpendWise].[PlanDetails] pd, [SpendWise].[Users] us where [pd].[created_by]=[us].[user_id] order by numberOfUse desc";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<BudgetPlanGetPopular>(sql).ToList();
            return file;
        }
    }
}
