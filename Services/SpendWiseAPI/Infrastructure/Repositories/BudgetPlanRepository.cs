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
        public List<BudgetPlan> GetPlans()
        {
            var sql = "select [plan_id], [name], [description], [noCategory], [category]  from [SpendWise].[PlanDetails]";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<BudgetPlan>(sql).ToList();
            return file;
        }
        public List<BudgetPlan> GetPlan(Guid id)
        {
            var sql = "select [plan_id], [name], [description], [noCategory], [category]  from [SpendWise].[PlanDetails] where [plan_id]=@PlanID";
            var connection = _databaseContext.GetDbConnection();
            var plan = connection.Query<BudgetPlan>(sql, new { PlanID = id }).ToList();
            return plan;
        }
    }
}
