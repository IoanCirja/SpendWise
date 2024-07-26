using Application.Interfaces;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MonthlyPlanService
    {
        private IMonthlyPlanRepository _monthlyPlanRepository;
        public MonthlyPlanService(IMonthlyPlanRepository monthlyPlanRepository)
        {
            _monthlyPlanRepository = monthlyPlanRepository;
        }
        public async Task<bool> AddMonthlyPlans(MonthlyPlan monthlyPlan)
        {
            return await _monthlyPlanRepository.AddMonthlyPlans(monthlyPlan);
        }
        public async Task<bool> CancelMonthlyPlan(Guid id)
        {
            return await _monthlyPlanRepository.CancelMonthlyPlan(id);
        }
        public List<MonthlyPlanGetNameDate> GetHistoryPlans(Guid user_id)
        {

            return _monthlyPlanRepository.GetHistoryPlans(user_id);
        }
        public List<MonthlyPlanGet> GetMonthlyPlanFromHistory(Guid monthlyPlan_id)
        {
            return _monthlyPlanRepository.GetMonthlyPlanFromHistory(monthlyPlan_id);
        }
        public List<MonthlyPlanGet> GetCurrentPlan(Guid user_id) 
        {
            var date = _monthlyPlanRepository.GetDateFromMonthlyPlanByUserID(user_id);
            if (date.Count == 0)
            {
                return new List<MonthlyPlanGet>();
            }
            if (date.Count != 1)
            {
                throw new Exception("errors, many plans in progress");
            }
            DateTime currentDate = DateTime.Now;
            TimeSpan difference = currentDate - date[0].date;
            if(Math.Abs(difference.Days) >=30 )
            {
                var result = _monthlyPlanRepository.FinishedMonthlyPlan(date[0].monthlyPlan_id);
                if (result == false)
                {
                    throw new Exception("errors, problem when finished a monthly plan");
                }
                return new List<MonthlyPlanGet>();
            }
            return _monthlyPlanRepository.GetCurrentPlan(user_id);
        }

        public MonthlyPlanDemo GetDemoMonthlyPlan(Guid plan_id)
        {
            var result = _monthlyPlanRepository.GetDemoMonthlyPlan(plan_id);
            if (result.Count==0)
            {
                throw new Exception("Errorrs, insert demo for plan_id not exist");
            }
            return result.FirstOrDefault();
        }
    }
}
