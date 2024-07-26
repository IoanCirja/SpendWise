using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Interfaces
{
    public interface IMonthlyPlanRepository
    {
        Task<bool> AddMonthlyPlans(MonthlyPlan monthlyPlan );
        Task<bool> CancelMonthlyPlan(Guid id);
        List<MonthlyPlanGetNameDate> GetHistoryPlans(Guid user_id);
        List<MonthlyPlanGet> GetMonthlyPlanFromHistory(Guid monthlyPlan_id);
        List<MonthlyPlanGet> GetCurrentPlan(Guid user_id);
        List<MonthlyPlanGetDateID> GetDateFromMonthlyPlanByUserID(Guid user_id);
        bool FinishedMonthlyPlan(Guid monthlyPlan_id);
        bool UpdateMonthlyPlanWithTransaction(Guid monthlyPlan_id, double amount, string spentOfCategory);

        Task<bool> CancelMonthlyPlansByPlanId(Guid id);
        List<MonthlyPlanDemo> GetDemoMonthlyPlan(Guid plan_id);
    }
}
