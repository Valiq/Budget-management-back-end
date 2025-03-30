using Budget_management_back_end.Core;
using Budget_management_back_end.Models;
using Microsoft.AspNetCore.Mvc;
using static Budget_management_back_end.Records.Records;

namespace Budget_management_back_end.Controllers
{
    [ApiController]
    public class FinanceEntityController : Controller
    {
        private readonly UserWorker Worker;

        public FinanceEntityController(IConfiguration configuration)
        {
            Worker = new UserWorker(configuration);
        }

        [HttpPost("api/v1/accounts/{id}/finance-entities")]
        public async Task<IActionResult> CreateEntity([FromRoute] long id, [FromBody] FinanceEntityRequest request)
        {
            FinanceEntity entity = new FinanceEntity();
            return CreatedAtAction(entity.Name, new { id = entity.Id }, entity);
        }

        [HttpGet("api/v1/accounts/{id}/finance-entities")]
        public async Task<IActionResult> GetEntityByAccount([FromRoute] long id)
        {

            return Ok(new { entities = new List<FinanceEntity>() });
        }

        [HttpPatch("api/v1/finance-entities/{id}")]
        public async Task<IActionResult> UpdateEntity([FromRoute] long id)
        {

            return Ok();
        }

        [HttpDelete("api/v1/finance-entities/{id}")]
        public async Task<IActionResult> DeleteEntity([FromRoute] long id)
        {

            return Ok();
        }
    }
}
