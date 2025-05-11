using Core.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProjectController : ControllerBase
    {
        private readonly UserProjectService _userProjectService;

        public UserProjectController(UserProjectService userProjectService)
        {
            _userProjectService = userProjectService;
        }

        // إضافة عضو إلى المشروع
        [HttpPost]
        public async Task<ActionResult<UserProject>> AddUserToProject([FromBody] UserProject userProject)
        {
            if (userProject == null)
            {
                return BadRequest("Invalid user project data.");
            }

            var addedUserProject = await _userProjectService.AddUserToProjectAsync(userProject);
            return CreatedAtAction(nameof(GetUsersByProjectId), new { projectId = addedUserProject.ProjectId }, addedUserProject);
        }

        // الحصول على أعضاء المشروع
        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<List<UserProject>>> GetUsersByProjectId(Guid projectId)
        {
            var userProjects = await _userProjectService.GetUsersByProjectIdAsync(projectId);
            return Ok(userProjects);
        }

        // إزالة عضو من المشروع
        [HttpDelete("{userId}/{projectId}")]
        public async Task<IActionResult> RemoveUserFromProject(Guid userId, Guid projectId)
        {
            var result = await _userProjectService.RemoveUserFromProjectAsync(userId, projectId);
            if (!result)
            {
                return NotFound("User not found in the project.");
            }
            return NoContent();
        }
    }

}
