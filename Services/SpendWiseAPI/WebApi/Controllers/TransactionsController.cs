using Application.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Transactions;
using WebApiContracts;
using WebApiContracts.Mappers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TransactionsController:ControllerBase
    {
        private readonly TransactionsService _transactionsService;
        public TransactionsController(TransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }
        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionsContract transaction)
        {
            var result = await _transactionsService.AddTransaction(transaction.MapTestToDomain());

            if (result)
            {
                return Ok("Transaction added successfully.");
            }

            return StatusCode(500, "An error occurred while adding the monthly plan.");
        }
        [HttpDelete ("{transaction_id}")]
        public async Task<IActionResult> DeleteTransactions(Guid transaction_id)
        {
            var result = await _transactionsService.DeleteTransactions(transaction_id);

            if (result)
            {
                return Ok("Transaction deleted successfully.");
            }

            return StatusCode(500, "An error occurred while adding the monthly plan.");
        }
        [HttpGet("{monthlyPlan_id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAllTransactions(Guid monthlyPlan_id)

        {
            var result = this._transactionsService.GetAllTransactions(monthlyPlan_id);

            return Ok(result);
        }
        [HttpGet("{transaction_id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetTransaction(Guid transaction_id)

        {
            var result = this._transactionsService.GetTransaction(transaction_id);

            return Ok(result);
        }
        [HttpGet("{category}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetTransactionForCategory(string category)

        {
            var result = this._transactionsService.GetTransactionForCategory(category);

            return Ok(result);
        }
    }
}
