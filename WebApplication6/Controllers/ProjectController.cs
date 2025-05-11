using Core.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _projectService;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        // إضافة مشروع
        [HttpPost]
        public async Task<ActionResult<Project>> AddProject([FromBody] Project project)
        {
            if (project == null)
            {
                return BadRequest("Invalid project data.");
            }

            var addedProject = await _projectService.AddProjectAsync(project);
            return CreatedAtAction(nameof(GetProjectById), new { projectId = addedProject.Id }, addedProject);
        }

        // الحصول على كل المشاريع
        [HttpGet]
        public async Task<ActionResult<List<Project>>> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        // الحصول على مشروع بواسطة ID
        [HttpGet("{projectId}")]
        public async Task<ActionResult<Project>> GetProjectById(Guid projectId)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return NotFound("Project not found.");
            }
            return Ok(project);
        }

        // تحديث مشروع
        [HttpPut("{projectId}")]
        public async Task<ActionResult<Project>> UpdateProject(Guid projectId, [FromBody] Project project)
        {
            if (project == null || project.Id != projectId)
            {
                return BadRequest("Project data is invalid.");
            }

            var updatedProject = await _projectService.UpdateProjectAsync(project);
            return Ok(updatedProject);
        }

        // حذف مشروع
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(Guid projectId)
        {
            var result = await _projectService.DeleteProjectAsync(projectId);
            if (!result)
            {
                return NotFound("Project not found.");
            }
            return NoContent();
        }
    }

}
