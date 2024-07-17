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
        List<MonthlyPlanGetNameDate> GetHistoryPlans(Guid user_id);
        List<MonthlyPlanGet> GetMonthlyPlanFromHistory(Guid monthlyPlan_id);
        List<MonthlyPlanGet> GetCurrentPlan(Guid user_id);

    }
}
