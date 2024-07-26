using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITransactionsRepository
    {
        Task<bool> AddTransaction(Transactions transactions);
        Task<bool> DeleteTransactions(Guid transaction_id);
        List<Transactions> GetAllTransactions(Guid monthlyPlan_id);
        List<Transactions> GetTransaction(Guid transaction_id);
        List<Transactions> GetTransactionsForCategory(string category);
        List<TransactionsInfo> GetBiggestTransaction(Guid user_id);
        List<TransactionsInfo> GetSmallestTransaction(Guid user_id);
        List<TransactionsInfo> GetBiggestTransactionCurrentPlan(Guid user_id);
        List<TransactionsInfo> GetSmallestTransactionCurrentPlan(Guid user_id);
        List<DateTime> GetDate(Guid monthlyPlan_id, string category, double Amount);
    }
}
