using Application.Interfaces;
using Domain;

namespace Application.Services
{
    public class BudgetPlanService
    {
        private IBudgetPlanRepository _budgetPlanRepository;
        private IMonthlyPlanRepository _monthlyPlanRepository;

        public BudgetPlanService(IBudgetPlanRepository budgetPlanRepository, IMonthlyPlanRepository monthlyPlanRepository)
        {
            _budgetPlanRepository = budgetPlanRepository;
            _monthlyPlanRepository = monthlyPlanRepository;
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

        public async Task<bool> AddNewPlan(BudgetPlan budgetPlan)
        {
            var planCheck = await this._budgetPlanRepository.GetPlanByName(budgetPlan.name);

            if (planCheck != null)
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

        //Added in 21/07/2024

        public List<BudgetPlan> GetPlansByAdminCreator(Guid id)
        {
            return _budgetPlanRepository.GetPlansByAdminCreator(id);
        }


        public async Task<BudgetPlan> EditPlanByPlanId(BudgetPlan budgetPlan, Guid id)
        {
            var planCheck = await this._budgetPlanRepository.GetPlanByName(budgetPlan.name);


            var plan = await this._budgetPlanRepository.GetPlanById(id);

            if (plan == null)
            {
                throw new Exception("Plan not found");
            }

            if (planCheck != null && plan.name != budgetPlan.name)
            {
                throw new Exception("Plan with this name already exists");
            }
            var result = await this._budgetPlanRepository.EditPlanById(budgetPlan, id);

            return result;
        }

        public async Task<BudgetPlan> EditPlanByPlanName(BudgetPlan budgetPlan, String name)
        {
            var plan = await this._budgetPlanRepository.GetPlanByName(name);

            var planCheck = await this._budgetPlanRepository.GetPlanByName(budgetPlan.name);

            if (plan == null)
            {
                throw new Exception("Plan not found");
            }

            if (planCheck != null && budgetPlan.name != name)
            {
                throw new Exception("Plan with this name already exists");
            }

            var result = await this._budgetPlanRepository.EditPlanByName(budgetPlan, name);

            return result;
        }

        public async Task<String> DeletePlanById(Guid id)
        {
            var plan = await this._budgetPlanRepository.GetPlanById(id);

            if (plan == null)
            {
                throw new Exception("Plan not found");
            }
            await this._monthlyPlanRepository.CancelMonthlyPlansByPlanId(id);
            var result = await this._budgetPlanRepository.DeletePlanById(id);

            return result;
        }

        public async Task<String> DeletePlanByName(String name)
        {
            var plan = await this._budgetPlanRepository.GetPlanByName(name);

            if (plan == null)
            {
                throw new Exception("Plan not found");
            }
            await this._monthlyPlanRepository.CancelMonthlyPlansByPlanId(plan.plan_id);
            var result = await this._budgetPlanRepository.DeletePlanByName(name);

            return result;
        }


        public List<BudgetPlan> GetActivePlans()
        {
            return _budgetPlanRepository.GetActivePlans();
        }
    }
}
