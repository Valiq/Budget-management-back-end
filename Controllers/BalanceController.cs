using Budget_management_back_end.Core;
using Budget_management_back_end.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Budget_management_back_end.Records.Records;

namespace Budget_management_back_end.Controllers
{
    [ApiController]
    public class BalanceController : Controller
    {
        private readonly BalanceWorker Worker;

        public BalanceController(IConfiguration configuration)
        {
            Worker = new BalanceWorker(configuration);
        }

        [HttpPost("api/v1/finance-entities/{id}/balances")]
        public async Task<IActionResult> CreateBalance([FromHeader] string Token, [FromRoute] long entityId, [FromBody] BalanceRequest request)
        {
            long id = Worker.AddBalance(entityId, request, Token);

            if (id != -1)
                return Ok(new { id });
            else
                return BadRequest();
        }

        [HttpGet("api/v1/finance-entities/{id}/balances")]
        public async Task<IActionResult> GetBalanceByEntity([FromHeader] string Token, [FromRoute] long id)
        {
            List<Balance> balances = Worker.GetBalanceByFinanceEntityId(id);

            if (balances is not null)
                return Ok(new { balances });
            else
                return NotFound();
        }

        [HttpGet("api/v1/balances/currencies")]
        public async Task<IActionResult> GetCurrency([FromHeader] string Token)
        {
            List<Currency> currencies = Worker.GetCurrency();

            if (currencies is not null)
            return Ok(new { currencies });
            else
                return NotFound();
        }

        [HttpDelete("api/v1/balances/{id}")]
        public async Task<IActionResult> DeleteBalance([FromHeader] string Token, [FromRoute] long id)
        {
            if (Worker.DeleteBalance(id, Token))
                return Ok();
            else
                return BadRequest();
        }
    }
}
