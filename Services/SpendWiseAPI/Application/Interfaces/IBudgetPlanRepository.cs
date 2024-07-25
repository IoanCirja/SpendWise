using Domain;


namespace Application.Interfaces
{
    public interface IBudgetPlanRepository
    {

        List<BudgetPlanGet> GetPlans();
        List<BudgetPlanGet> GetPlan(Guid id);
        List<BudgetPlanGetPopular> GetPopularFivePlans();


        Task<IEnumerable<BudgetPlan>> GetPlanByName(string name);
        Task<bool> AddPlan(BudgetPlan budgetPlan);

        //Added 21/07/2024

        List<BudgetPlan> GetPlansByAdminCreator(Guid id);

        Task<BudgetPlan> GetPlanById(Guid id);

        Task<BudgetPlan> EditPlanById(BudgetPlan budgetPlan, Guid id);

        Task<BudgetPlan> EditPlanByName(BudgetPlan budgetPlan, String name);

        Task<String> DeletePlanById(Guid id);

        Task<String> DeletePlanByName(String name);
    }
}
