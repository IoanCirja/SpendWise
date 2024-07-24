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
    }
}
