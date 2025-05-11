using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    // Core/Domain/Projects/UserProject.cs
    public class UserProject
    {
        public Guid? UserId { get; set; }
        public Guid ProjectId { get; set; }
        public ProjectRole Role { get; set; } = ProjectRole.Contributor;

        // العلاقات
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
    }

    public enum ProjectRole
    {
        Viewer,      // مشاهدة فقط
        Contributor, // إضافة/تعديل المهام
        Admin        // إدارة المشروع بالكامل
    }
}
