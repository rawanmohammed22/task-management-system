using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces;
public interface ISubTaskService
    {
        Task<SubTask> CreateSubTaskAsync(string title, string description, Guid taskId);
        Task<SubTask> GetSubTaskByIdAsync(Guid id);
        Task<IEnumerable<SubTask>> GetSubTasksByTaskIdAsync(Guid taskId);
        Task<bool> UpdateSubTaskAsync(Guid id, string title, string description, bool isCompleted);
        Task<bool> DeleteSubTaskAsync(Guid id);
    }


