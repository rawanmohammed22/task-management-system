using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    // TaskTracker.Core/Domain/Users/ApplicationUser.cs
   

    public class ApplicationUser : IdentityUser<Guid>, IAuditableEntity
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }


        public virtual ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();

        // العلاقات مع المهام
        public ICollection<TaskItem> CreatedTasks { get; set; } = new List<TaskItem>();
        public ICollection<TaskItem> AssignedTasks { get; set; } = new List<TaskItem>();

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public virtual ICollection<Attacht> Attachments { get; set; } = new List<Attacht>();


        public DateTime? LastModified { get ; set; }
    }
}
