using Application.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiContracts;
using WebApiContracts.Mappers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MonthlyPlanController : ControllerBase
    {
        private readonly MonthlyPlanService _monthlyPlanService;

        public MonthlyPlanController(MonthlyPlanService monthlyPlanService)
        {
            _monthlyPlanService = monthlyPlanService;
        }

        [HttpPost]
        public async Task<IActionResult> AddMonthlyPlans([FromBody] MonthlyPlanContract monthlyPlan)
        {
            var result = await _monthlyPlanService.AddMonthlyPlans(monthlyPlan.MapTestToDomain());

            if (result)
            {
                return Ok("Monthly plan added successfully.");
            }

            return StatusCode(500, "An error occurred while adding the monthly plan.");
        }
        [HttpPost]
        public async Task<IActionResult> CancelMonthlyPlans([FromBody] Guid id)
        {
            var result = await _monthlyPlanService.CancelMonthlyPlan(id);

            if (result)
            {
                return Ok("Monthly plan canceled successfully.");
            }

            return StatusCode(500, "An error occurred while adding the monthly plan.");
        }

        [HttpGet ("{user_id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetHistoryPlans( Guid user_id)

        {
            var result = this._monthlyPlanService.GetHistoryPlans(user_id);

            return Ok(result);
        }


        [HttpGet ("{monthlyPlanid}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetPlanFromHistory( Guid monthlyPlanid)

        {
            var result = this._monthlyPlanService.GetMonthlyPlanFromHistory(monthlyPlanid);

            return Ok(result);
        }


        [HttpGet ("{user_id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetCurrentPlan( Guid user_id)

        {
            var result = this._monthlyPlanService.GetCurrentPlan(user_id);

            return Ok(result);
        }
        [HttpGet("{plan_id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetDemoMonthlyPlan(Guid plan_id)

        {
            var result = this._monthlyPlanService.GetDemoMonthlyPlan(plan_id);

            return Ok(result);
        }
    }
}