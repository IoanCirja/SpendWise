using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult> GetFile()
        {
            var result = this._planService.GetPlans();

            return Ok(result);
        }

    }
}
