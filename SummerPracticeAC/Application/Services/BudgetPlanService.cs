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


        public async Task<bool> AddNewPlan(BudgetPlan budgetPlan)
        {
            var planCheck = await this._budgetPlanRepository.GetPlan(budgetPlan.Name);

            if (planCheck.ToList().Count != 0)
            {
                throw new Exception("Plan already added");
            }
            var registerResult = await this._budgetPlanRepository.AddPlan(new BudgetPlan
            {
                Name = budgetPlan.Name,
                Description = budgetPlan.Description,
                Category = budgetPlan.Category,
                NoCategory = budgetPlan.NoCategory,
                CreatedBy = budgetPlan.CreatedBy,
                Imagine = budgetPlan.Imagine,
            });

            return registerResult;
        }




    }
}
