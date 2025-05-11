using Core.DTOs;
using Core.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        private Guid GetUserIdFromToken()
        {
            try
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    throw new UnauthorizedAccessException("Invalid or missing Bearer token");
                }

                var token = authorizationHeader.Substring("Bearer ".Length).Trim();
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    throw new UnauthorizedAccessException("User ID claim not found in token");
                }

                if (!Guid.TryParse(userIdClaim, out var userId))
                {
                    throw new UnauthorizedAccessException("Invalid User ID format in token");
                }

                return userId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting user ID from token");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var result = await _taskService.CreateTaskAsync(dto, userId);
                return Ok(new
                {
                    Message = "Done",
                    TaskId = result.Id,
                    Status = result.Status
                });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Null DTO in create task");
                return BadRequest("Task data is required");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error in create task");
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access");
                return Unauthorized(ex.Message);
            }
            catch (InvalidOperationException ex) when (ex.InnerException is DbUpdateException)
            {
                _logger.LogError(ex, "Database error in create task");
                return StatusCode(500, "Database operation failed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in create task");
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TaskDto>> GetById(Guid id)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var task = await _taskService.GetTaskDetailsAsync(id, userId);
                return task == null ? NotFound() : Ok(task);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting task with ID {id}");
                return StatusCode(500, "An error occurred while retrieving the task");
            }
        }

        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetMyTasks()
        {
            try
            {
                var userId = GetUserIdFromToken();
                var tasks = await _taskService.GetUserTasksAsync(userId);
                return Ok(tasks);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user tasks");
                return StatusCode(500, "An error occurred while retrieving user tasks");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskDto dto)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var updated = await _taskService.UpdateTaskAsync(id, dto, userId);
                return updated ? NoContent() : NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating task with ID {id}");
                return StatusCode(500, "An error occurred while updating the task");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var deleted = await _taskService.DeleteTaskAsync(id, userId);
                return deleted ? NoContent() : NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting task with ID {id}");
                return StatusCode(500, "An error occurred while deleting the task");
            }
        }

        [HttpPost("{id:guid}/assign/{assigneeId:guid}")]
        public async Task<IActionResult> Assign(Guid id, Guid assigneeId)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var success = await _taskService.AssignTaskAsync(id, assigneeId, userId);
                return success ? NoContent() : NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error assigning task {id} to user {assigneeId}");
                return StatusCode(500, "An error occurred while assigning the task");
            }
        }

        [HttpPost("{id:guid}/status")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeStatusDto dto)
        {
            try
            {
                var userId = GetUserIdFromToken();
                var success = await _taskService.ChangeTaskStatusAsync(id, dto.Status, userId);
                return success ? NoContent() : NotFound();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error changing status for task {id}");
                return StatusCode(500, "An error occurred while changing task status");
            }
        }
    }
}