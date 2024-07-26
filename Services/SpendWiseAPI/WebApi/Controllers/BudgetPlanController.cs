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

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetPlan(Guid id)
        {
            var result = this._planService.GetPlan(id);

            return Ok(result);
        }



        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetPopularFivePlans()
        {
            var result = this._planService.GetPopularFivePlans();
            return Ok(result);
        }







        //Added 21/07/2024

        [HttpGet ("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetPlansByAdminCreator(Guid id)
        {
            var result = this._planService.GetPlansByAdminCreator(id);

            return Ok(result);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> AddPlan([FromBody] BudgetPlanContract budgetPlanContract)
        {
            var result = await this._planService.AddNewPlan(budgetPlanContract.MapTestToDomain());
            return Ok(result);
        }

        [HttpPost ("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> EditPlanByPlanId([FromBody] BudgetPlanEditContract budgetPlanContract, Guid id)
        {
            var result = await this._planService.EditPlanByPlanId(budgetPlanContract.MapPlanEditToDomain(), id);
            return Ok(result);
        }

        [HttpPost ("{name}")]
        [AllowAnonymous]
        public async Task<ActionResult> EditPlanMyName([FromBody] BudgetPlanEditContract budgetPlanContract, String name)
        {
            var result = await this._planService.EditPlanByPlanName(budgetPlanContract.MapPlanEditToDomain(), name);
            return Ok(result);
        }

        [HttpDelete ("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> DeletePlanById(Guid id)
        {
            var result = await this._planService.DeletePlanById(id);
            return Ok(result);
        }

        [HttpDelete ("{name}")]
        [AllowAnonymous]
        public async Task<ActionResult> DeletePlanByName(String name)
        {
            var result = await this._planService.DeletePlanByName(name);
            return Ok(result);
        }


    }
}
