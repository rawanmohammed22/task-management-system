using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Entities
{
    // Core/Domain/Tasks/Comment.cs
    public class Comment : IAuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid TaskId { get; set; }
        public Guid AuthorId { get; set; }

        // العلاقات
        [ForeignKey("TaskId")]
        public virtual TaskItem Task { get; set; }

        [ForeignKey("AuthorId")]
        public virtual ApplicationUser Author { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
