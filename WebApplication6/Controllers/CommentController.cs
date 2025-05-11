using Core.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }

        // إضافة تعليق
        [HttpPost]
        public async Task<ActionResult<Comment>> AddComment([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return BadRequest("Invalid comment data.");
            }

            var addedComment = await _commentService.AddCommentAsync(comment);
            return CreatedAtAction(nameof(GetComments), new { taskId = addedComment.TaskId }, addedComment);
        }

        // الحصول على تعليقات بناءً على TaskId
        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<List<Comment>>> GetComments(Guid taskId)
        {
            var comments = await _commentService.GetCommentsByTaskIdAsync(taskId);
            if (comments == null || !comments.Any())
            {
                return NotFound("No comments found for this task.");
            }
            return Ok(comments);
        }

        // حذف تعليق
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var result = await _commentService.DeleteCommentAsync(commentId);
            if (!result)
            {
                return NotFound("Comment not found.");
            }
            return NoContent();
        }
    }

}
