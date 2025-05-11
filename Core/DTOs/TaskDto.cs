using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using TaskStatus = Core.Entities.TaskStatus;

namespace Core.DTOs
{
    public class TaskDto
    {

        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // كنص بدلًا من enum
        public DateTime DueDate { get; set; }

        // بدل العلاقات الكاملة - نستخدم IDs فقط
        public Guid CreatorId { get; set; }
        public Guid? AssigneeId { get; set; }
        public Guid ProjectId { get; set; }
    }
}
