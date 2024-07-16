using Domain;


namespace Application.Interfaces
{
    public interface IBudgetPlanRepository
    {
        List<BudgetPlan> GetPlans();
        Task<IEnumerable<BudgetPlan>> GetPlan(string name);
        Task<bool> AddPlan(BudgetPlan budgetPlan);



    }
}
