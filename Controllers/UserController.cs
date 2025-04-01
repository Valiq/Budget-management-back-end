using Budget_management_back_end.Core;
using Budget_management_back_end.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using System.ComponentModel.DataAnnotations;
using static Budget_management_back_end.Records.Records;
using static Budget_management_back_end.Core.AuthorizationWorker;

namespace Budget_management_back_end.Controllers
{
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserWorker Worker;

        public UserController(IConfiguration configuration)
        {
            Worker = new UserWorker(configuration);
        }

        [HttpPost("api/v1/users")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest request)
        {
            long id = Worker.AddUser(request);

            if (id != -1)
                return CreatedAtAction(request.name, new { id });
            else
                return BadRequest();
        }

        [HttpPost("api/v1/session")]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            string token = Worker.LoginUser(request);

            if (!string.IsNullOrEmpty(token))
                return Ok(new { Token = token });
            else
                return BadRequest();
        }

        [HttpGet("api/v1/users/email")]
        public async Task<IActionResult> GetUserByEmail([FromQuery][Required] string email)
        {
            var user = Worker.GetUserByEmail(email);

            if (user is not null)
                return Ok(user);
            else
                return NotFound();
        }

        [HttpGet("api/v1/users/token")]
        public async Task<IActionResult> GetUserByToken([FromHeader] string Token)
        {
            var user = Worker.GetUserByToken(Token);

            if (user is not null)
                return Ok(user);
            else
                return NotFound();
        }

        [HttpDelete("api/v1/users/{id}")]
        public async Task<IActionResult> DeleteUser([FromHeader] string Token, [FromRoute] long id)
        {
           if (Worker.DeleteUserById(id, Token))
                return Ok();
           else 
                return BadRequest();
        }

        [HttpPatch("api/v1/users/{id}/email")]
        public async Task<IActionResult> UpdateEmail([FromHeader] string Token, [FromRoute] long id, [FromBody] UpdateEmailRequest request)
        {
            string token = Worker.UpdateUserEmail(id, request.email, Token);

            if (!string.IsNullOrEmpty(token))
                return Ok();
            else
                return BadRequest();
        }


        [HttpPatch("api/v1/users/{id}/password")]
        public async Task<IActionResult> UpdatePassword([FromHeader] string Token, [FromRoute] long id, [FromBody] UpdatePasswordRequest request)
        {
            string token = Worker.UpdateUserPassword(id, request.password, Token);

            if (!string.IsNullOrEmpty(token))
                return Ok();
            else
                return BadRequest();
        }

        [HttpPatch("api/v1/users/{id}")]
        public async Task<IActionResult> UpdateUser([FromHeader] string Token, [FromRoute] long id, [FromBody] UpdaeUserRequest request)
        {
            User user = new User()
            {
                Id = id,
                Name = request.name
            };

            if (Worker.UpdateUser(user, Token))
                return Ok();
            else
                return BadRequest();
        }
    }
}