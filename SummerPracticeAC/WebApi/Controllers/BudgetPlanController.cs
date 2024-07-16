using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApiContracts.Mappers;
using WebApiContracts;

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
        public async Task<ActionResult> GetAllPlans()
        {
            var result = this._planService.GetPlans();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        public async Task<ActionResult<bool>> AddPlan([FromQuery] BudgetPlanContract budgetPlanContract)
        {
            var userId = User.FindFirstValue(ClaimTypes.Email);
            Console.WriteLine(userId + " -----------------------");
            Guid id_user = new Guid("BFA5C58A-1DD0-4C67-89E0-D82224B02472");
            var result = await this._planService.AddNewPlan(budgetPlanContract.MapTestToDomain(id_user));
            return Ok(result);
        }


    }
}
