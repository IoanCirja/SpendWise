using Application.Interfaces;
using Domain;

namespace Application.Services
{
    public class BudgetPlanService
    {
        private IBudgetPlanRepository _budgetPlanRepository;
        private IMonthlyPlanRepository _monthlyPlanRepository;
        private IAuthenticationRepository _authenticationRepository;
        private IEmailSender _emailSender;


        public BudgetPlanService(IBudgetPlanRepository budgetPlanRepository, IMonthlyPlanRepository monthlyPlanRepository,IAuthenticationRepository authenticationRepository,IEmailSender emailSender)
        {
            _budgetPlanRepository = budgetPlanRepository;
            _monthlyPlanRepository = monthlyPlanRepository;
            _authenticationRepository = authenticationRepository;
            _emailSender = emailSender;
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
            var registerResult = await this._budgetPlanRepository.AddPlan(budgetPlan);


            if (registerResult)
            {

                string imageDataUrl = "https://i.postimg.cc/HntvP2Pk/logo.png";
                var emails = await this._authenticationRepository.GetAllUsersEmails();

                string subject = "A New Budget Plan was added on the SpendWise Website ";


                string body = $@"
                            <html>
                            <body>
                            <img src='{imageDataUrl}' alt='Logo' />
                            <p><strong>A new budget plan named '{budgetPlan.name}' has been added.</strong></p>
                            <p>Description: {budgetPlan.description}</p>
                            <p>The budget plan has <strong>{budgetPlan.noCategory}</strong> categories:</p>
                            <ul>";


                var categories = budgetPlan.category.Split(',');


                foreach (var category in categories)
                {
                    body += $"<li>{category.Trim()}</li>";
                }
                body += "</ul></body></html>";



                await this._emailSender.SendEmailAsync(emails, subject, body);
            }

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

    }
}
