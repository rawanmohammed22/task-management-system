using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Entities
{
    // Core/Domain/Tasks/TaskItem.cs
    public class TaskItem : IAuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium; 
        public TaskStatus Status { get; set; } = TaskStatus.Todo;
        public DateTime DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public bool IsRecurring { get; set; } = false;
        public string? RecurrencePattern { get; set; }

        // Foreign Keys
        public Guid ProjectId { get; set; }
        

        // العلاقات
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        // Foreign Keys
        public Guid CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }

        // Assignee relationship
        public Guid? AssigneeId { get; set; }  // nullable in case task is unassigned
        public ApplicationUser Assignee { get; set; }

        public virtual ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();
        public virtual ICollection<Attacht> Attachments { get; set; } = new List<Attacht>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public DateTime? LastModified { get; set; }

    }

    public enum TaskPriority
    {
        Low, Medium, High, Critical
    }

    public enum TaskStatus
    {
        Todo, InProgress, Done, Archived
    }
}
