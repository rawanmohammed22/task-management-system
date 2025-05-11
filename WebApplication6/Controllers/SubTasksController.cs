using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubTasksController : ControllerBase
    {
        private readonly ISubTaskService _subTaskService;

        public SubTasksController(ISubTaskService subTaskService)
        {
            _subTaskService = subTaskService;
        }

        [HttpPost]
        public async Task<ActionResult<SubTask>> Create([FromBody] CreateSubTaskRequest request)
        {
            var subTask = await _subTaskService.CreateSubTaskAsync(request.Title, request.Description, request.TaskId);
            return Ok(subTask);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<SubTask>> GetById(Guid id)
        {
            var subTask = await _subTaskService.GetSubTaskByIdAsync(id);
            return subTask != null ? Ok(subTask) : NotFound();
        }

        [HttpGet("task/{taskId:guid}")]
        public async Task<ActionResult<IEnumerable<SubTask>>> GetByTaskId(Guid taskId)
        {
            var subTasks = await _subTaskService.GetSubTasksByTaskIdAsync(taskId);
            return Ok(subTasks);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSubTaskRequest request)
        {
            var result = await _subTaskService.UpdateSubTaskAsync(id, request.Title, request.Description, request.IsCompleted);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _subTaskService.DeleteSubTaskAsync(id);
            return result ? NoContent() : NotFound();
        }
    }

}
