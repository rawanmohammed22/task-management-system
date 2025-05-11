using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    // Core/Domain/Common/IAuditableEntity.cs
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; set; }
        DateTime? LastModified { get; set; }
    }
}
