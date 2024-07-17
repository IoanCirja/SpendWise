using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiContracts;
using WebApiContracts.Mappers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class BudgetPlanController : ControllerBase
    {

        private BudgetPlanService _planService;
        public BudgetPlanController(BudgetPlanService planService)
        {
            _planService = planService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetPlans()
        {
            var result = this._planService.GetPlans();

            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetPlan([FromQuery] Guid id)
        {
            var result = this._planService.GetPlan(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddPlan([FromQuery] BudgetPlanContract budgetPlanContract)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user1 = User.FindFirstValue(ClaimTypes.Name);
            var user3 = User.FindFirstValue(ClaimTypes.GivenName);
            var user2 = User.FindFirstValue(ClaimTypes.Email);


            Console.WriteLine("-------------" + user + " -----------------------" + "Name" + user1 + "Email" + user2 + "Given Name" + user3);

            Guid id_user = new Guid("BFA5C58A-1DD0-4C67-89E0-D82224B02472");
            var result = await this._planService.AddNewPlan(budgetPlanContract.MapTestToDomain());
            return Ok(result);
        }
    }
}
