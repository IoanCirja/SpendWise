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
            return _monthlyPlanRepository.GetCurrentPlan(user_id);
        }
    }
}
