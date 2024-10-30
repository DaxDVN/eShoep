using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Identity.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController(UserManager<User> userManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userManager.Users.ToListAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            var user = new User { UserName = request.Email, Email = request.Email };
            var result = await userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                return Ok(user);
            }

            return BadRequest(result.Errors);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = request.Email;
            user.UserName = request.Email;

            if (!string.IsNullOrEmpty(request.Password))
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, token, request.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }

            var updateResult = await userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                return Ok(user);
            }

            return BadRequest(updateResult.Errors);
        }
    }

    public record CreateUserRequest(string Email, string Password);

    public record UpdateUserRequest(string Email, string Password);
}