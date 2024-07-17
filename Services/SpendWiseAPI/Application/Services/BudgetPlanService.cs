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

        public async Task<bool> AddNewPlan(BudgetPlan budgetPlan)
        {
            var planCheck = await this._budgetPlanRepository.GetPlanByName(budgetPlan.name);

            if (planCheck.ToList().Count != 0)
            {
                throw new Exception("Plan already added");
            }
            var registerResult = await this._budgetPlanRepository.AddPlan(new BudgetPlan
            {
                name = budgetPlan.name,
                description = budgetPlan.description,
                category = budgetPlan.category,
                noCategory = budgetPlan.noCategory,
                created_by = budgetPlan.created_by,
                image = budgetPlan.image
            });

            return registerResult;
        }





    }
}
