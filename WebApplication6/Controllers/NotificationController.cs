using Core.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // إضافة إشعار
        [HttpPost]
        public async Task<ActionResult<Notification>> AddNotification([FromBody] Notification notification)
        {
            if (notification == null)
            {
                return BadRequest("Invalid notification data.");
            }

            var addedNotification = await _notificationService.AddNotificationAsync(notification);
            return CreatedAtAction(nameof(GetNotificationsByUserId), new { userId = addedNotification.UserId }, addedNotification);
        }

        // الحصول على إشعارات المستخدم
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Notification>>> GetNotificationsByUserId(Guid userId)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            return Ok(notifications);
        }

        // تحديث حالة الإشعار
        [HttpPut("{notificationId}/markAsRead")]
        public async Task<IActionResult> MarkNotificationAsRead(Guid notificationId)
        {
            var result = await _notificationService.MarkNotificationAsReadAsync(notificationId);
            if (!result)
            {
                return NotFound("Notification not found.");
            }
            return NoContent();
        }
    }

}
