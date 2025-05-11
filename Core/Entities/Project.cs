using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Core.Entities
{
    // Core/Domain/Projects/Project.cs
    public class Project : IAuditableEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? OwnerId { get; set; }  // <-- nullable

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

       
        [ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }
        public virtual ICollection<UserProject> TeamMembers { get; set; } = new List<UserProject>();
        public virtual ICollection<TaskItem> Tasko { get; set; } = new List<TaskItem>();
        public DateTime? LastModified { get; set; }
    }
}
