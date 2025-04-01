using Budget_management_back_end.Core;
using Budget_management_back_end.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using static Budget_management_back_end.Records.Records;

namespace Budget_management_back_end.Controllers
{
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly TransactionWorker Worker;

        public TransactionController(IConfiguration configuration)
        {
            Worker = new TransactionWorker(configuration);
        }

        [HttpPost("api/v1/transactions")]
        public async Task<IActionResult> CreateTransaction([FromHeader] string Token, [FromBody] TransactionRequest request)
        {
            if (Worker.AddTransaction(request, Token))
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet("api/v1/accounts/{id}/transactions")]
        public async Task<IActionResult> GetAccountTransaction([FromHeader] string Token, [FromRoute] long id)
        {
            List<TransactionAudit> audit = Worker.GetAccountTransaction(id, Token);

            if (audit is not null)
                return Ok(audit);
            else
                return NotFound();
        }
    }
}
