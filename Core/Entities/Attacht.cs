using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    // Core/Domain/Tasks/Attachment.cs
    public class Attacht


    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
       
        public Guid? UploaderId { get; set; }
        public  ApplicationUser Uploader { get; set; }

        public Guid TaskId { get; set; } // مفتاح أجنبي (FK)
        public TaskItem Tasks { get; set; }



    }
}
