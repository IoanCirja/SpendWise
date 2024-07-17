using Application.Interfaces;
using Domain;

namespace Application.Services
{
    public class BudgetPlanService
    {
        private IBudgetPlanRepository _budgetPlanRepository;


        public BudgetPlanService(IBudgetPlanRepository budgetPlanRepository)
        {
            _budgetPlanRepository = budgetPlanRepository;
        }

        public List<BudgetPlanGet> GetPlans()
        {

            return _budgetPlanRepository.GetPlans();
        }
        public List<BudgetPlanGet> GetPlan(Guid id)
        {
            return _budgetPlanRepository.GetPlan(id);
        }
        public List<BudgetPlanGetPopular> GetPopularFivePlans()
        {
            return _budgetPlanRepository.GetPopularFivePlans();

        }
    }
}
