using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ISubTaskRepository 
    {
        Task<SubTask> AddAsync(SubTask subTask);
        Task<SubTask> GetByIdAsync(Guid id);
        Task<IEnumerable<SubTask>> GetByTaskIdAsync(Guid taskId);
        Task UpdateAsync(SubTask subTask);
        Task DeleteAsync(SubTask subTask); 
    }
}

