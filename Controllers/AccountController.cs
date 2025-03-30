using Budget_management_back_end.Core;
using Budget_management_back_end.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.ComponentModel.DataAnnotations;
using static Budget_management_back_end.Records.Records;

namespace Budget_management_back_end.Controllers
{
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserWorker Worker;

        public AccountController(IConfiguration configuration)
        {
            Worker = new UserWorker(configuration);
        }

        [HttpPost("api/v1/users/{id}/accounts")]
        public async Task<IActionResult> CreateAccount([FromRoute] long id, [FromQuery][Required] string role, [FromBody] AccountRequest request)
        {
            Account account = new Account();
            return CreatedAtAction(account.Name, new { id = account.Id }, account);
        }

        [HttpPost("api/v1/accounts/{id}/users")]
        public async Task<IActionResult> AddUserAccount([FromRoute] long id, [FromQuery][Required] string email)
        {

            return Ok();
        }

        [HttpGet("api/v1/users/{id}/accounts")]
        public async Task<IActionResult> GetAccount([FromRoute] long id)
        {

            return Ok(new { accaunts = new List<Account>() });
        }

        [HttpGet("api/v1/accounts/{id}/users")]
        public async Task<IActionResult> GetUserAccount([FromRoute] long id)
        {

            return Ok(new { users = new List<UserRequest>() });
        }

        [HttpGet("api/v1/users/roles")]
        public async Task<IActionResult> GetRole()
        {

            return Ok(new { roles = new List<Role>() });
        }

        [HttpPatch("api/v1/accounts/{id}")]
        public async Task<IActionResult> UpdateAccount([FromRoute] long id, [FromBody] UpdateAccountRequest request)
        {

            return Ok();
        }

        [HttpDelete("api/v1/accounts/{id}")]
        public async Task<IActionResult> DeleteAccount([FromRoute] long id)
        {

            return Ok();
        }

        [HttpDelete("api/v1/accounts/{id}/users")]
        public async Task<IActionResult> DeleteUserAccount([FromRoute] long id,[FromQuery][Required] string email)
        {

            return Ok();
        }
    }
}
