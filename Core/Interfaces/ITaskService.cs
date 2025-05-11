using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskStatus = Core.Entities.TaskStatus;
using Core.DTOs;

namespace Core.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(CreateTaskDto dto, Guid creatorId);
        Task<TaskDto?> GetTaskDetailsAsync(Guid taskId, Guid userId);
        Task<IEnumerable<TaskDto>> GetUserTasksAsync(Guid userId);
        Task<bool> UpdateTaskAsync(Guid taskId, UpdateTaskDto dto, Guid userId);
        Task<bool> DeleteTaskAsync(Guid taskId, Guid userId);
        Task<bool> AssignTaskAsync(Guid taskId, Guid assigneeId, Guid currentUserId);
        Task<bool> ChangeTaskStatusAsync(Guid taskId, TaskStatus status, Guid userId);
    }
}
