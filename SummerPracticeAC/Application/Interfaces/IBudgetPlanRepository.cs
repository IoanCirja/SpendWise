using Domain;


namespace Application.Interfaces
{
    public interface IBudgetPlanRepository
    {
        List<BudgetPlan> GetPlans();
    }
}
