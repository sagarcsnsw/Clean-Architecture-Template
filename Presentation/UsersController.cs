using Microsoft.AspNetCore.Mvc;
using YourApp.Application.DTOs;
using YourApp.Application.Services;

namespace YourApp.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // ✅ Extract arguments from HTTP request
        [HttpGet("with-hobbies")]
        public async Task<IActionResult> GetUsersWithHobbies()
        {
            try
            {
                // ✅ Execute use case via direct service call
                var users = await _userService.GetUsersWithHobbiesAsync();
                
                // ✅ Return appropriate HTTP status code and data
                return Ok(new
                {
                    success = true,
                    data = users,
                    timestamp = DateTime.UtcNow
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                // ✅ Return appropriate error message and status
                return Forbid(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // ✅ Handle unexpected errors
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpGet("{id}/with-hobbies")]
        public async Task<IActionResult> GetUserWithHobbies(int id)
        {
            try
            {
                // ✅ Extract arguments from route
                var user = await _userService.GetUsersWithHobbiesAsync();
                var specificUser = user.FirstOrDefault(u => u.UserId == id);
                
                return specificUser != null 
                    ? Ok(new { success = true, data = specificUser })
                    : NotFound(new { message = "User not found" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDto request)
        {
            try
            {
                // ✅ Extract arguments from HTTP request body
                if (request == null)
                    return BadRequest(new { message = "Request body is required" });

                // ✅ Execute use case
                var user = await _userService.CreateUserAsync(request);
                
                // ✅ Return appropriate status code and location
                return CreatedAtAction(
                    nameof(GetUserWithHobbies), 
                    new { id = user.Id }, 
                    new { success = true, data = user });
            }
            catch (ArgumentException ex)
            {
                // ✅ Return validation error
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        [HttpPut("parent/{parentId}/update-students")]
        public async Task<IActionResult> UpdateStudentsWhenParentChanges(int parentId)
        {
            try
            {
                // ✅ Extract arguments from route
                await _userService.UpdateStudentsWhenParentChangesAsync(parentId);
                
                return Ok(new { 
                    success = true, 
                    message = "Students updated successfully",
                    timestamp = DateTime.UtcNow 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}