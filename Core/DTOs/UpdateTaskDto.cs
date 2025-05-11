using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.DTOs
{
   public  class UpdateTaskDto
    {

        [StringLength(100)]
        public string? Title { get; set; }

        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriority? Priority { get; set; } 
    }
}
