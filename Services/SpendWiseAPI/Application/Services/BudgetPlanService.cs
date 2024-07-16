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

        public List<BudgetPlan> GetPlans()
        {

            return _budgetPlanRepository.GetPlans();
        }
        public List<BudgetPlan> GetPlan(Guid id)
        {
            return _budgetPlanRepository.GetPlan(id);
        }




    }
}
