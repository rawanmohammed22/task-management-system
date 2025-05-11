using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
    using TaskStatus = Core.Entities.TaskStatus;


namespace Core.DTOs
{
  public class ChangeStatusDto
    {
        public TaskStatus Status { get; set; } 
    }
}
