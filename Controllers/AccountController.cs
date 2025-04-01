using Budget_management_back_end.Core;
using Budget_management_back_end.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using static Budget_management_back_end.Records.Records;

namespace Budget_management_back_end.Controllers
{
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AccountWorker Worker;

        public AccountController(IConfiguration configuration)
        {
            Worker = new AccountWorker(configuration);
        }

        [HttpPost("api/v1/users/{id}/accounts")]
        public async Task<IActionResult> CreateAccount([FromHeader] string Token, [FromRoute] long userId, [FromBody] AccountRequest request)
        {
            long id = Worker.AddAccount(userId, request, Token);

            if (id != -1)
                return Ok( new { Id = id });
            else
                return BadRequest();
        }

        [HttpPost("api/v1/accounts/{id}/users")]
        public async Task<IActionResult> AddUserAccount([FromHeader] string Token, [FromRoute] long id, [FromQuery][Required] string email, [FromQuery][Required] string role)
        {
            if (Worker.AddUserAccount(id, email, role, Token))
                return Ok();
            else
                return BadRequest();
        }

        [HttpGet("api/v1/accounts/{id}")]
        public async Task<IActionResult> GetAccount([FromRoute] long id)
        {
            Account account = Worker.GetAccount(id);

            if (account is not null)
                return Ok(account);
            else
                return NotFound();
        }

        [HttpGet("api/v1/users/{id}/accounts")]
        public async Task<IActionResult> GetUserAccount([FromRoute] long id)
        {
            List<Account> accounts = Worker.GetUserAccount(id);

            if (accounts is not null)
                return Ok(accounts);
            else
                return NotFound();
        }

        [HttpGet("api/v1/users/roles")]
        public async Task<IActionResult> GetRole()
        {
            List<Role> roles = Worker.GetRole();

            if (roles is not null)
                return Ok(roles);
            else 
                return NotFound();
        }

        [HttpPatch("api/v1/accounts/{id}")]
        public async Task<IActionResult> UpdateAccount([FromHeader] string Token, [FromRoute] long id, [FromBody] UpdateAccountRequest request)
        {
            Account account = new Account() 
            { 
                Id = id,
                Name = request.name,
                Description = request.description
            };

            if (Worker.UpdateAccount(account, Token))
                return Ok();
            else
                return BadRequest();
        }

        [HttpDelete("api/v1/accounts/{id}")]
        public async Task<IActionResult> DeleteAccount([FromHeader] string Token, [FromRoute] long id)
        {
            if (Worker.DeleteAccount(id, Token))
                return Ok();
            else
                return BadRequest();
        }

        [HttpDelete("api/v1/accounts/{id}/users")]
        public async Task<IActionResult> DeleteUserAccount([FromHeader] string Token, [FromRoute] long id,[FromQuery][Required] string email)
        {
            if (Worker.DeleteUserAccountByEmail(id, email, Token))
                return Ok();
            else
                return BadRequest();
        }
    }
}
