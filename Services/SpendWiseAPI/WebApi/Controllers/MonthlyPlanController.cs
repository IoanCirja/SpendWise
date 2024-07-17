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
        public async Task<IActionResult> AddMonthlyPlans([FromQuery] MonthlyPlanContract monthlyPlan)
        {
            var result = await _monthlyPlanService.AddMonthlyPlans(monthlyPlan.MapTestToDomain());

            if (result)
            {
                return Ok("Monthly plan added successfully.");
            }

            return StatusCode(500, "An error occurred while adding the monthly plan.");
        }
        [HttpPost]
        public async Task<IActionResult> CancelMonthlyPlans([FromQuery] Guid id)
        {
            var result = await _monthlyPlanService.CancelMonthlyPlan(id);

            if (result)
            {
                return Ok("Monthly plan canceled successfully.");
            }

            return StatusCode(500, "An error occurred while adding the monthly plan.");
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetMonthlyPlans([FromQuery] Guid user_id)
        {
            var result = this._monthlyPlanService.GetMonthlyPlans(user_id);

            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetMonthlyPlan([FromQuery] Guid monthlyPlanid)
        {
            var result = this._monthlyPlanService.GetMonthlyPlan(monthlyPlanid);

            return Ok(result);
        }
    }
}
