using Budget_management_back_end.Core;
using Budget_management_back_end.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static Budget_management_back_end.Records.Records;

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
            User user = new User();
            return CreatedAtAction(user.Name, new { id = user.Id }, user);
        }

        [HttpPost("api/v1/session")]
        public async Task<IActionResult> Login([FromBody] Login request)
        {
            // Логика авторизации
            return Ok(new { Token = "..." });
        }

        [HttpGet("api/v1/users/email")]
        public async Task<IActionResult> GetUserByEmail([FromQuery][Required] string email)
        {

            return Ok(new { Id = 0 });
        }

        [HttpGet("api/v1/users/token")]
        public async Task<IActionResult> GetUserByToken()
        {

            return Ok(new { Id = 0 });
        }

        [HttpDelete("api/v1/users/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] long id)
        {

            return Ok();
        }

        [HttpPatch("api/v1/users/{id}/email")]
        public async Task<IActionResult> UpdateEmail([FromRoute] long id, [FromBody] UpdateEmailRequest request)
        {

            return Ok();
        }


        [HttpPatch("api/v1/users/{id}/password")]
        public async Task<IActionResult> UpdatePassword([FromRoute] long id, [FromBody] UpdatePasswordRequest request)
        {

            return Ok();
        }

        [HttpPatch("api/v1/users/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] long id, [FromBody] UpdaeUserRequest request)
        {

            return Ok();
        }
    }
}
