using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TransactionsService
    {
        private ITransactionsRepository _transactionsRepository;
        public TransactionsService(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository; 
        }
        public async Task<bool> AddTransaction(Transactions transactions)
        {
            return await _transactionsRepository.AddTransaction(transactions);
        }
        public async Task<bool> DeleteTransactions(Guid transaction_id)
        {
            return await _transactionsRepository.DeleteTransactions(transaction_id);    
        }
        public List<Transactions> GetAllTransactions(Guid monthlyPlan_id)
        {
            return _transactionsRepository.GetAllTransactions(monthlyPlan_id);
        }
        public List<Transactions> GetTransaction(Guid transaction_id)
        {
            return _transactionsRepository.GetTransaction(transaction_id);
        }
    }
}
