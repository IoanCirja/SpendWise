using Application.Interfaces;
using Dapper;
using Domain;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TransactionsRepository: ITransactionsRepository
    {
        private readonly IDatabaseContext _databaseContext;
        public TransactionsRepository(IDatabaseContext databaseContext)
        {
            this._databaseContext = databaseContext;
        }
        public async Task<bool> AddTransaction(Transactions transactions)
        {
            var query = "INSERT INTO [SpendWise].[Transactions] ([transaction_id], [name], [monthlyPlan_id], [date], [category], [amount]) VALUES (NEWID(), @Name, @MonthlyPlanID, @Date, @Category, @Amount)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", transactions.name, DbType.String);
            parameters.Add("MonthlyPlanID", transactions.monthlyPlan_id, DbType.Guid);
            parameters.Add("Date", transactions.date, DbType.DateTime);
            parameters.Add("Category", transactions.category, DbType.String);
            parameters.Add("Amount", transactions.amount, DbType.Int64);


            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, parameters, _databaseContext.GetDbTransaction());
            return result != 0;
        }
        public async Task<bool> DeleteTransactions(Guid transaction_id)
        {
            var query = "DELETE FROM [SpendWise].[Transactions]  where [transaction_id]=@TransactionID";
            var connection = _databaseContext.GetDbConnection();
            var result = await connection.ExecuteAsync(query, new { TransactionID = transaction_id });
            return result != 0;
        }
        public List<Transactions> GetAllTransactions(Guid monthlyPlan_id)
        {
            var sql = "select [transaction_id], [name], [monthlyPlan_id], [date], [category], [amount] from [SpendWise].[Transactions] where [monthlyPlan_id]=@MonthlyPlanID";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<Transactions>(sql, new { MonthlyPlanID = monthlyPlan_id }).ToList();
            return file;
        }
        public List<Transactions> GetTransaction(Guid transaction_id)
        {
            var sql = "select [transaction_id], [name], [monthlyPlan_id], [date], [category], [amount] from [SpendWise].[Transactions] where [transaction_id]=@TransactionID";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<Transactions>(sql, new { TransactionID = transaction_id }).ToList();
            return file;
        }

        public List<Transactions> GetTransactionsForCategory(string category)
        {
            var sql = "select [transaction_id], [name], [monthlyPlan_id], [date], [category], [amount] from [SpendWise].[Transactions] where [category]=@Category";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<Transactions>(sql, new { Category = category }).ToList();
            return file;
        }

        public List<TransactionsInfo> GetBiggestTransaction(Guid user_id)
        {
            var sql = "select top(1) [t].[name], max([t].[amount]) as 'amount' from [SpendWise].[Transactions] t, [SpendWise].[MonthlyPlan] mp where [t].[monthlyPlan_id]=[mp].[monthlyPlan_id] and [mp].[user_id]=@UserID group by [t].[name] having max([t].[amount])=(select max([tr].[amount]) from [SpendWise].[Transactions] tr, [SpendWise].[MonthlyPlan] mpl where [tr].[monthlyPlan_id] = [mpl].[monthlyPlan_id] and [mpl].[user_id]=@UserID);";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<TransactionsInfo>(sql, new { UserID = user_id }).ToList();
            return file;
        }

        public List<TransactionsInfo> GetSmallestTransaction(Guid user_id)
        {
            var sql = "select top(1) [t].[name], min([t].[amount]) as 'amount' from [SpendWise].[Transactions] t, [SpendWise].[MonthlyPlan] mp where [t].[monthlyPlan_id]=[mp].[monthlyPlan_id] and [mp].[user_id]=@UserID group by [t].[name] having min([t].[amount])=(select min([tr].[amount]) from [SpendWise].[Transactions] tr, [SpendWise].[MonthlyPlan] mpl where [tr].[monthlyPlan_id] = [mpl].[monthlyPlan_id] and [mpl].[user_id]=@UserID);";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<TransactionsInfo>(sql, new { UserID = user_id }).ToList();
            return file;
        }
        public List<TransactionsInfo> GetBiggestTransactionCurrentPlan(Guid user_id)
        {
            var sql = "select top(1) [t].[name], max([t].[amount]) as 'amount' from [SpendWise].[Transactions] t, [SpendWise].[MonthlyPlan] mp where [t].[monthlyPlan_id]=[mp].[monthlyPlan_id] and [mp].[user_id]=@UserID and [mp].[status]='In Progress' group by [t].[name] having max([t].[amount])=(select max([tr].[amount]) from [SpendWise].[Transactions] tr, [SpendWise].[MonthlyPlan] mpl where [tr].[monthlyPlan_id] = [mpl].[monthlyPlan_id] and [mpl].[user_id]=@UserID);";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<TransactionsInfo>(sql, new { UserID = user_id }).ToList();
            return file;
        }
        public List<TransactionsInfo> GetSmallestTransactionCurrentPlan(Guid user_id)
        {
            var sql = "select top(1) [t].[name], min([t].[amount]) as 'amount' from [SpendWise].[Transactions] t, [SpendWise].[MonthlyPlan] mp where [t].[monthlyPlan_id]=[mp].[monthlyPlan_id] and [mp].[user_id]=@UserID and [mp].[status]='In Progress' group by [t].[name] having min([t].[amount])=(select min([tr].[amount]) from [SpendWise].[Transactions] tr, [SpendWise].[MonthlyPlan] mpl where [tr].[monthlyPlan_id] = [mpl].[monthlyPlan_id] and [mpl].[user_id]=@UserID);";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<TransactionsInfo>(sql, new { UserID = user_id }).ToList();
            return file;
        }
        public List<DateTime> GetDate(Guid monthlyPlan_id, string category, double Amount)
        {
            Console.WriteLine(category +Amount);
            var sql = "WITH CategorySums AS (" +
                    "SELECT [date],SUM(amount) OVER (PARTITION BY monthlyPlan_id, category ORDER BY date) AS cumulative_amount " +
                    "FROM SpendWise.Transactions where monthlyPlan_id = @MonthlyPlanID and (category=@Category or CONCAT(' ',category)=@Category) )" +
                    "SELECT ISNULL(MIN(date),CONVERT(DATETIME,'2500-10-10',120)) as 'data' FROM CategorySums WHERE cumulative_amount >= @Amount ";
            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<DateTime>(sql, new { MonthlyPlanID = monthlyPlan_id, Category = category, Amount = Amount });
            Console.WriteLine(file.FirstOrDefault());
            if (file.Count() == 0)
            {
                Console.WriteLine("Nu s-au găsit rezultate.");
            }

            return file.ToList();
        }
        public List<Transactions> GetAllTransactionsForUser(Guid user_id)
        {
            var sql = "select [t].[transaction_id], [t].[name], [t].[monthlyPlan_id], [t].[date], [t].[category], [t].[amount] from [SpendWise].[Transactions] t, [SpendWise].[MonthlyPlan] mp where [t].[monthlyPlan_id]=[mp].[monthlyPlan_id] and [mp].[user_id]=@UserID";

            var connection = _databaseContext.GetDbConnection();
            var file = connection.Query<Transactions>(sql, new { UserID = user_id }).ToList();
            return file;
        }
    }
}
