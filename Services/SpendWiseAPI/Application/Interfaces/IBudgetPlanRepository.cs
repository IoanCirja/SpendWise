using Domain;


namespace Application.Interfaces
{
    public interface IBudgetPlanRepository
    {

        List<BudgetPlanGet> GetPlans();
        List<BudgetPlanGet> GetPlan(Guid id);
        List<BudgetPlanGetPopular> GetPopularFivePlans();

        List<BudgetPlan> GetPlans();
        List<BudgetPlan> GetPlan(Guid id);
        Task<IEnumerable<BudgetPlan>> GetPlanByName(string name);
        Task<bool> AddPlan(BudgetPlan budgetPlan);

    }
}
