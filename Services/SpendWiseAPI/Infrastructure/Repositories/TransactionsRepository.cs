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
            parameters.Add("Date", transactions.date, DbType.Date);
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
    }
}
