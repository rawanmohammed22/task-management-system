using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Entities
{
    // Core/Domain/Notifications/Notification.cs
    public class Notification : IAuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public NotificationType Type { get; set; }
        public Guid UserId { get; set; }
        public Guid? RelatedEntityId { get; set; } // Could be TaskId, ProjectId etc.

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public DateTime? LastModified { get; set; }
    }

    public enum NotificationType
    {
        TaskAssigned,
        TaskDueSoon,
        TaskCompleted,
        NewComment,
        ProjectInvitation,
        MentionInComment
    }
}
