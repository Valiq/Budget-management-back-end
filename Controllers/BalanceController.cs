using Budget_management_back_end.Core;
using Budget_management_back_end.Models;
using Microsoft.AspNetCore.Mvc;
using static Budget_management_back_end.Records.Records;

namespace Budget_management_back_end.Controllers
{
    [ApiController]
    public class BalanceController : Controller
    {
        private readonly UserWorker Worker;

        public BalanceController(IConfiguration configuration)
        {
            Worker = new UserWorker(configuration);
        }

        [HttpPost("api/v1/finance-entities/{id}/balances")]
        public async Task<IActionResult> CreateBalance([FromRoute] long id, [FromBody] BalanceRequest request)
        {
            Balance balance = new Balance();
            return CreatedAtAction(balance.Name, new { id = balance.Id }, balance);
        }

        [HttpGet("api/v1/finance-entities/{id}/balances")]
        public async Task<IActionResult> GetBalanceByEntity([FromRoute] long id)
        {

            return Ok(new { balances = new List<Balance>() });
        }

        [HttpGet("api/v1/balances/currencies")]
        public async Task<IActionResult> GetCurrency()
        {

            return Ok(new { currencies = new List<Currency>() });
        }

        [HttpPatch("api/v1/balances/{id}")]
        public async Task<IActionResult> UpdateBalance([FromRoute] long id)
        {

            return Ok();
        }

        [HttpDelete("api/v1/balances/{id}")]
        public async Task<IActionResult> DeleteBalance([FromRoute] long id)
        {

            return Ok();
        }
    }
}
