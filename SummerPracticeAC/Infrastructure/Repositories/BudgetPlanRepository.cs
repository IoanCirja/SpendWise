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

        public Task<IEnumerable<BudgetPlan>> GetPlan(string name)
        {
            var sql = "SELECT * from [SpendWise].[PlanDetails] where [name] = @name";
            var connection = _databaseContext.GetDbConnection();
            var plan = connection.QueryAsync<BudgetPlan>(sql,new {Name = name});
            return plan;
        }

        public async Task<bool> AddPlan(BudgetPlan budgetPlan)
        {
            var query = "INSERT INTO [SpendWise].[PlanDetails] ([plan_id], [name], [description], [category], [noCategory], [created_by], [imagine]) VALUES (NEWID(), @Name, @Description, @Category, @NoCategory, @Created_by,@Imagine)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", budgetPlan.Name, DbType.String);
            parameters.Add("Description", budgetPlan.Description, DbType.String);
            parameters.Add("Category", budgetPlan.Category, DbType.String);
            parameters.Add("NoCategory", budgetPlan.NoCategory, DbType.Int64);
            parameters.Add("Created_by", budgetPlan.CreatedBy, DbType.Guid);
            parameters.Add("Imagine", budgetPlan.Imagine, DbType.String);
            

            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }
    }
}
