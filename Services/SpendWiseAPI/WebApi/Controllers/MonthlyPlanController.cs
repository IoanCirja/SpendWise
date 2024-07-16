using Application.Services;
using Domain;
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
    }
}
