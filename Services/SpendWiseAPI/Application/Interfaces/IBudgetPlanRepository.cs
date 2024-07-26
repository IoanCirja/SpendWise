using Domain;


namespace Application.Interfaces
{
    public interface IBudgetPlanRepository
    {

        List<BudgetPlanGet> GetPlans();
        List<BudgetPlanGet> GetPlan(Guid id);
        List<BudgetPlanGetPopular> GetPopularFivePlans();
        List<BudgetPlanGetPopular> GetMostUsedPlan(Guid user_id);




        Task<bool> AddPlan(BudgetPlan budgetPlan);

        List<BudgetPlan> GetPlansByAdminCreator(Guid id);

        Task<BudgetPlan> GetPlanById(Guid id);

        Task<BudgetPlan> EditPlanById(BudgetPlan budgetPlan, Guid id);

        Task<BudgetPlan> EditPlanByName(BudgetPlan budgetPlan, String name);

        Task<String> DeletePlanById(Guid id);

        Task<String> DeletePlanByName(String name);
        Task<BudgetPlan> GetPlanByName(string name);
        List<BudgetPlan> GetActivePlans();
    }
}
