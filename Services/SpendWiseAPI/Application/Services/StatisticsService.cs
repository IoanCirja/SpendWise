using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StatisticsService
    {
        private IBudgetPlanRepository _budgetPlanRepository;
        private IMonthlyPlanRepository _monthlyPlanRepository;
        private ITransactionsRepository _transactionsRepository;
        private IParserData _parserData;
        public StatisticsService(IBudgetPlanRepository budgetPlanRepository, IMonthlyPlanRepository monthlyPlanRepository, ITransactionsRepository transactionsRepository, IParserData parserData)
        {
            _budgetPlanRepository = budgetPlanRepository;
            _monthlyPlanRepository = monthlyPlanRepository;
            _transactionsRepository = transactionsRepository;
            _parserData = parserData;
        }

        public Statistics GetStatistics(Guid user_Id)
        {
            var resultBudgetPlan = _budgetPlanRepository.GetMostUsedPlan(user_Id);
            var resultBiggestTransaction = _transactionsRepository.GetBiggestTransaction(user_Id);
            var resultSmallestTransaction = _transactionsRepository.GetSmallestTransaction(user_Id);
            var resultBiggestTransactionCurrentPlan = _transactionsRepository.GetBiggestTransactionCurrentPlan(user_Id);
            var resultSmallestTransactionCurrentPlan = _transactionsRepository.GetSmallestTransactionCurrentPlan(user_Id);
            var resultCurrentPlan = _monthlyPlanRepository.GetCurrentPlan(user_Id);
            string[] categories;
            double[] prices;
            string categorie = "";
            if (resultCurrentPlan.Count()!=0)
            {
                categories = _parserData.GetCategory(resultCurrentPlan.FirstOrDefault().category);
                prices = _parserData.GetPrice(resultCurrentPlan.FirstOrDefault().priceByCategory);

                int i = 0;
                DateTime? minDate = null;
                categorie = "";
                foreach (var category in categories)
                {
                    var resultData = _transactionsRepository.GetDate(resultCurrentPlan.FirstOrDefault().monthlyPlan_id, category, prices[i]);
                    if (resultData.Count != 0 && (!minDate.HasValue || resultData.FirstOrDefault() < minDate.Value))
                    {
                        minDate = resultData.FirstOrDefault();
                        categorie = category.ToString();
                    }
                    i++;
                }
                if (minDate.ToString() == "10.10.2500 00:00:00")
                {
                    categorie = "not full category";
                }
            }
            Statistics result = new Statistics();
            result.mostUsedPlanName = resultBudgetPlan.FirstOrDefault()?.name??"No plan in history";
            result.biggestTransactionNameForever = resultBiggestTransaction.FirstOrDefault()?.name??"No transaction in history";
            result.biggestTransactionAmountForever = resultBiggestTransaction.FirstOrDefault()?.amount??0;
            result.smallestTransactionNameForever = resultSmallestTransaction.FirstOrDefault()?.name??"No transaction in history";
            result.smallestTransactionAmountForever = resultSmallestTransaction.FirstOrDefault()?.amount??0;
            result.biggestTransactionNameCurrentPlan = resultBiggestTransactionCurrentPlan.FirstOrDefault()?.name??"No transaction in current plan";
            result.biggestTransactionAmountCurrentPlan = resultBiggestTransactionCurrentPlan.FirstOrDefault()?.amount??0;
            result.smallestTransactionNameCurrentPlan = resultSmallestTransactionCurrentPlan.FirstOrDefault()?.name??"No transaction in current plan";
            result.smallestTransactionAmountCurrentPlan = resultSmallestTransactionCurrentPlan.FirstOrDefault()?.amount??0;
            result.firstCategoryFull = categorie.ToString();
            result.TotalMoney = resultCurrentPlan.FirstOrDefault()?.totalAmount??0;
            result.SpendMoney = resultCurrentPlan.FirstOrDefault()?.amountSpent??0;

            return result;
        }
    }
}
