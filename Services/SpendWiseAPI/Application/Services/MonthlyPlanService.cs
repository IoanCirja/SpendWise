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
        public List<MonthlyPlanGetNameDate> GetMonthlyPlans(Guid user_id)
        {

            return _monthlyPlanRepository.GetMonthlyPlans(user_id);
        }
        public List<MonthlyPlanGet> GetMonthlyPlan(Guid monthlyPlan_id)
        {
            return _monthlyPlanRepository.GetMonthlyPlan(monthlyPlan_id);
        }
    }
}
