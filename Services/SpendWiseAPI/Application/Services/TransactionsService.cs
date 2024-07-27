using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Application.Services
{
    public class TransactionsService
    {
        private ITransactionsRepository _transactionsRepository;
        private IMonthlyPlanRepository _monthlyPlanRepository;
        private IBudgetPlanRepository _budgetPlanRepository;
        private IParserData _parserData;
        public TransactionsService(ITransactionsRepository transactionsRepository, IMonthlyPlanRepository monthlyPlanRepository, IBudgetPlanRepository budgetPlanRepository, IParserData parserData)
        {
            _transactionsRepository = transactionsRepository;
            _monthlyPlanRepository = monthlyPlanRepository;
            _budgetPlanRepository = budgetPlanRepository;
            _parserData = parserData;
        }
        public async Task<bool> AddTransaction(Transactions transactions)
        {
            var monthlyPlan = _monthlyPlanRepository.GetMonthlyPlanFromHistory(transactions.monthlyPlan_id);
            if (monthlyPlan.Count() == 0)
            {
                throw new Exception("monthy plan id not exist");
            }
            var spentOfCategory = _parserData.GetUpdatedStringSpentOfCategoryWhenAddTransaction(monthlyPlan.FirstOrDefault().category, monthlyPlan.FirstOrDefault().spentOfCategory, transactions.category, transactions.amount);
            if (spentOfCategory == "")
            {
                throw new Exception("parsing number error");
            }
            _monthlyPlanRepository.UpdateMonthlyPlanWithTransaction(transactions.monthlyPlan_id, transactions.amount, spentOfCategory);
            return await _transactionsRepository.AddTransaction(transactions);
        }
        public async Task<bool> DeleteTransactions(Guid transaction_id)
        {
            var transactions = _transactionsRepository.GetTransaction(transaction_id);
            if (transactions.Count() == 0)
            {
                throw new Exception("errors: transaction id not found");
            }
            var monthlyPlan = _monthlyPlanRepository.GetMonthlyPlanFromHistory(transactions.FirstOrDefault().monthlyPlan_id);
            if (monthlyPlan.Count() == 0)
            {
                throw new Exception("monthy plan id not exist");
            }
            var spentOfCategory = _parserData.GetUpdatedStringSpentOfCategoryWhenAddTransaction(monthlyPlan.FirstOrDefault().category, monthlyPlan.FirstOrDefault().spentOfCategory, transactions.FirstOrDefault().category, -transactions.FirstOrDefault().amount);
            if (spentOfCategory == "")
            {
                throw new Exception("parsing number error");
            }
            _monthlyPlanRepository.UpdateMonthlyPlanWithTransaction(transactions.FirstOrDefault().monthlyPlan_id, -transactions.FirstOrDefault().amount, spentOfCategory);
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
        public List<Transactions> GetTransactionForCategory(string category)
        {
            return _transactionsRepository.GetTransactionsForCategory(category);
        }
        public List<Transactions> GetAllTransactionForUser(Guid userId)
        {
            return _transactionsRepository.GetAllTransactionsForUser(userId);
        }
    }
}
