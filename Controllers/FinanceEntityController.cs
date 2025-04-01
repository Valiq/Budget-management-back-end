using Budget_management_back_end.Core;
using Budget_management_back_end.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Budget_management_back_end.Records.Records;

namespace Budget_management_back_end.Controllers
{
    [ApiController]
    public class FinanceEntityController : Controller
    {
        private readonly FinanceEntityWorker Worker;

        public FinanceEntityController(IConfiguration configuration)
        {
            Worker = new FinanceEntityWorker(configuration);
        }

        [HttpPost("api/v1/accounts/{id}/finance-entities")]
        public async Task<IActionResult> CreateEntity([FromHeader] string Token, [FromRoute] long accountId, [FromBody] FinanceEntityRequest request)
        {
            long id = Worker.AddFinanceEntity(accountId, request, Token);

            if (id != -1)
                return CreatedAtAction(request.name, new { id });
            else
                return BadRequest();
        }

        [HttpGet("api/v1/accounts/{id}/finance-entities")]
        public async Task<IActionResult> GetEntityByAccount([FromHeader] string Token, [FromRoute] long id)
        {
            List <FinanceEntity> entities = Worker.GetFinanceEntityByAccountId(id);

            if(entities is not null)
                return Ok(entities);
            else
                return NotFound();
        }

        [HttpPatch("api/v1/finance-entities/{id}")]
        public async Task<IActionResult> UpdateEntity([FromHeader] string Token, [FromRoute] long id, [FromBody] UpdateFinanceEntityRequest request)
        {
            FinanceEntity entity = new FinanceEntity()
            {
                Name = request.name,
                Description = request.description
            };

            if (Worker.UpdateFinanceEntity(entity, Token))
                return Ok();
            else
                return BadRequest();
        }

        [HttpDelete("api/v1/finance-entities/{id}")]
        public async Task<IActionResult> DeleteEntity([FromHeader] string Token, [FromRoute] long id)
        {
            if (Worker.DeleteFinanceEntity(id, Token))
                return Ok();
            else 
                return BadRequest();
        }
    }
}
