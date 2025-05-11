using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    // Core/Domain/Tasks/SubTask.cs
    public class SubTask
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public Guid TaskId { get; set; }

        [ForeignKey("TaskId")]
        public virtual TaskItem Task { get; set; }
    }
}
