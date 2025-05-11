using Core.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachController : ControllerBase
    {
        private readonly AttachService _attachService;

        public AttachController(AttachService attachService)
        {
            _attachService = attachService;
        }

        // إضافة مرفق
        [HttpPost]
        public async Task<ActionResult<Attacht>> AddAttachment([FromBody] Attacht attachment)
        {
            if (attachment == null)
            {
                return BadRequest("Invalid attachment data.");
            }

            var addedAttachment = await _attachService.AddAttachmentAsync(attachment);
            return CreatedAtAction(nameof(GetAttachments), new { taskId = addedAttachment.TaskId }, addedAttachment);
        }

        // الحصول على مرفقات بناءً على TaskId
        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<List<Attacht>>> GetAttachments(Guid taskId)
        {
            var attachments = await _attachService.GetAttachmentsByTaskIdAsync(taskId);
            if (attachments == null || !attachments.Any())
            {
                return NotFound("No attachments found for this task.");
            }
            return Ok(attachments);
        }

        // حذف مرفق
        [HttpDelete("{attachmentId}")]
        public async Task<IActionResult> DeleteAttachment(Guid attachmentId)
        {
            var result = await _attachService.DeleteAttachmentAsync(attachmentId);
            if (!result)
            {
                return NotFound("Attachment not found.");
            }
            return NoContent();
        }
    }

}
