using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
   public class CreateTaskDto

    {
        [Required, StringLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        public Guid? AssigneeId { get; set; } 
        public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(7);
    }
}
