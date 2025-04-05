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
        public async Task<IActionResult> CreateBalance([FromHeader] string Token, [FromRoute] long id, [FromBody] BalanceRequest request)
        {
            long balanceId = Worker.AddBalance(id, request, Token);

            if (balanceId != -1)
                return Ok(new { balanceId });
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
        public async Task<IActionResult> GetCurrency()
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
