using Application.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Interfaces;

using System.Data;


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

            var sql = "select [pd].[plan_id], [pd].[name], [pd].[description], [pd].[noCategory], [pd].[category], [pd].[image], [us].[name] as 'created_by',[pd].[isActive], [pd].[creationDate]  from [SpendWise].[PlanDetails] pd, [SpendWise].[Users] us where [pd].[created_by]=[us].[user_id] AND [pd].[isActive] = 1";


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
            var sql = "select top(5) [pd].[plan_id], [pd].[name], [pd].[description], [pd].[noCategory], [pd].[category], [pd].[image], (select count([mp].[plan_id]) from [SpendWise].[MonthlyPlan] mp where [mp].[plan_id] = [pd].[plan_id] and [isActive]=1 group by [mp].[plan_id]) as 'numberOfUse'  from [SpendWise].[PlanDetails] pd, [SpendWise].[Users] us where [pd].[created_by]=[us].[user_id] order by numberOfUse desc";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<BudgetPlanGetPopular>(sql).ToList();
            return file;
        }

        public List<BudgetPlanGetPopular> GetMostUsedPlan(Guid user_id)
        {
            var sql = "SELECT pd.plan_id, pd.name, pd.description, pd.noCategory, pd.category, pd.image, COUNT(mp.plan_id) AS numberOfUse FROM SpendWise.PlanDetails pd INNER JOIN SpendWise.MonthlyPlan mp ON pd.plan_id = mp.plan_id INNER JOIN SpendWise.Users us ON pd.created_by = us.user_id WHERE pd.isActive = 1 AND mp.user_id = @UserID GROUP BY pd.plan_id, pd.name, pd.description, pd.noCategory, pd.category, pd.image HAVING COUNT(mp.plan_id) > 0 ORDER BY numberOfUse DESC;";
            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<BudgetPlanGetPopular>(sql, new {UserID = user_id}).ToList();
            return file;
        }

        public async Task<BudgetPlan> GetPlanByName(string name)
        {
            var sql = "SELECT * FROM [SpendWise].[PlanDetails] WHERE [name] = @name";
            var connection = _databaseContext.GetDbConnection();
            var plan = await connection.QueryFirstOrDefaultAsync<BudgetPlan>(sql, new { Name = name });
            return plan;
        }

        public async Task<bool> AddPlan(BudgetPlan budgetPlan)
        {
            var query = "INSERT INTO [SpendWise].[PlanDetails] ([plan_id], [name], [description], [category], [noCategory], [created_by], [image]) VALUES (NEWID(), @Name, @Description, @Category, @NoCategory, @Created_by,@Image)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", budgetPlan.name, DbType.String);
            parameters.Add("Description", budgetPlan.description, DbType.String);
            parameters.Add("Category", budgetPlan.category, DbType.String);
            parameters.Add("NoCategory", budgetPlan.noCategory, DbType.Int64);
            parameters.Add("Created_by", budgetPlan.created_by, DbType.Guid);
            parameters.Add("Image", budgetPlan.image, DbType.String);
            parameters.Add("CreationDate", DateTime.Today, DbType.Date);
            parameters.Add("IsActive", 1, DbType.Int32);


            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }




        public List<BudgetPlan> GetPlansByAdminCreator(Guid id)
        {
            var query = "select [plan_id], [name], [description], [noCategory], [category], [image], [created_by], [isActive]  from [SpendWise].[PlanDetails] where [created_by]=@AdminId AND [isActive] = 1";

            var connection = _databaseContext.GetDbConnection();
            var result = connection.Query<BudgetPlan>(query, new { AdminId = id }).ToList();
            return result;
        }

        public async Task<BudgetPlan> GetPlanById(Guid id)
        {
            var sql = "SELECT [plan_id], [name], [description], [noCategory], [category], [image], [created_by] FROM [SpendWise].[PlanDetails] WHERE [plan_id] = @PlanID";
            var connection = _databaseContext.GetDbConnection();
            var plan = await connection.QuerySingleOrDefaultAsync<BudgetPlan>(sql, new { PlanID = id });
            return plan;
        }

        public async Task<BudgetPlan> EditPlanById(BudgetPlan budgetPlan, Guid id)
        {
            var sql = "UPDATE [SpendWiseDB].[SpendWise].[PlanDetails] SET [name] = @Name, [description] = @Description, [noCategory] = @NoCategory, [category] = @Category, [image] = @Image WHERE [plan_id] = @PlanID";

            var parameters = new DynamicParameters();
            parameters.Add("Name", budgetPlan.name, DbType.String);
            parameters.Add("Description", budgetPlan.description, DbType.String);
            parameters.Add("NoCategory", budgetPlan.noCategory, DbType.Int64);
            parameters.Add("Category", budgetPlan.category, DbType.String);
            parameters.Add("Image", budgetPlan.image, DbType.String);
            parameters.Add("PlanID", id, DbType.Guid);

            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(sql, parameters, _databaseContext.GetDbTransaction());

            if (result == 0)
            {
                throw new InvalidOperationException("No rows were updated. Plan ID may not exist.");
            }

            var query = "SELECT * FROM [SpendWiseDB].[SpendWise].[PlanDetails] WHERE [plan_id] = @PlanID";
            var plan = await connection.QuerySingleOrDefaultAsync<BudgetPlan>(query, new { PlanID = id });

            if (plan == null)
            {
                throw new InvalidOperationException("The plan was updated but could not be retrieved.");
            }

            return plan;
        }

        public async Task<BudgetPlan> EditPlanByName(BudgetPlan budgetPlan, String name)
        {
            var sql = "UPDATE [SpendWiseDB].[SpendWise].[PlanDetails] SET [name] = @NewName, [description] = @Description, [noCategory] = @NoCategory, [category] = @Category, [image] = @Image WHERE [name] = @Name";

            var parameters = new DynamicParameters();
            parameters.Add("NewName", budgetPlan.name, DbType.String);
            parameters.Add("Description", budgetPlan.description, DbType.String);
            parameters.Add("NoCategory", budgetPlan.noCategory, DbType.Int64);
            parameters.Add("Category", budgetPlan.category, DbType.String);
            parameters.Add("Image", budgetPlan.image, DbType.String);
            parameters.Add("Name", name, DbType.String);


            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(sql, parameters, _databaseContext.GetDbTransaction());

            if (result == 0)
            {
                throw new InvalidOperationException("No rows were updated. A Plan with this name may not exist.");
            }

            var query = "SELECT * FROM [SpendWiseDB].[SpendWise].[PlanDetails] WHERE [name] = @NewName";
            var plan = await connection.QuerySingleOrDefaultAsync<BudgetPlan>(query, new { NewName = budgetPlan.name });

            if (plan == null)
            {
                throw new InvalidOperationException("The plan was updated but could not be retrieved.");
            }

            return plan;
        }


        public async Task<string> DeletePlanById(Guid id)
        {
            var sql = "UPDATE [SpendWiseDB].[SpendWise].[PlanDetails] SET [isActive] = 0 WHERE [plan_id] = @PlanID";

            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(sql, new { PlanID = id });

            if (result > 0)
            {
                return "Plan with Id: " + id + " was deleted";
            }
            else
            {
                throw new InvalidOperationException("Plan with the specified id does not exist");
            }
        }

        public async Task<string> DeletePlanByName(String name)
        {
            var sql = "UPDATE [SpendWiseDB].[SpendWise].[PlanDetails] SET [isActive] = 0 WHERE [name] = @Name";

            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(sql, new { Name = name });

            if (result > 0)
            {
                return "Plan with Name: " + name + " was deleted";
            }
            else
            {
                throw new InvalidOperationException("Plan with the specified name does not exist");
            }
        }
    }
}
