using Domain;


namespace Application.Interfaces
{
    public interface IBudgetPlanRepository
    {
        List<BudgetPlanGet> GetPlans();
        List<BudgetPlanGet> GetPlan(Guid id);
        List<BudgetPlanGetPopular> GetPopularFivePlans();
    }
}
