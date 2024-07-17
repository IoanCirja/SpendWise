using Domain;


namespace Application.Interfaces
{
    public interface IBudgetPlanRepository
    {
        List<BudgetPlan> GetPlans();
        List<BudgetPlan> GetPlan(Guid id);
        Task<IEnumerable<BudgetPlan>> GetPlanByName(string name);
        Task<bool> AddPlan(BudgetPlan budgetPlan);
    }
}
