using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMonthlyPlanRepository
    {
        Task<bool> AddMonthlyPlans(MonthlyPlan monthlyPlan );
        Task<bool> CancelMonthlyPlan(Guid id);
        List<MonthlyPlan> GetMonthlyPlans(Guid user_id);
        List<MonthlyPlan> GetMonthlyPlan(Guid monthlyPlan_id);
    }
}
