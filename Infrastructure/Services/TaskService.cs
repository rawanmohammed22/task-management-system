using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using TaskStatus = Core.Entities.TaskStatus;

namespace Infrastructure.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto, Guid creatorId)
        {
            // التحقق من null للمدخلات
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Title is required");

            // التحقق من أن الـ repository غير null
            if (_taskRepository == null)
                throw new InvalidOperationException("Task repository is not initialized");

            var task = new Core.Entities.TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                ProjectId = dto.ProjectId,
                CreatorId = creatorId,
                AssigneeId = dto.AssigneeId,
                DueDate = dto.DueDate,
                Status = TaskStatus.Todo, // تغيير من Archived إلى New
                CreatedAt = DateTime.UtcNow
            };

            await _taskRepository.AddAsync(task);
            // تأكد من وجود هذه الدالة

            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                ProjectId = task.ProjectId,
                AssigneeId = task.AssigneeId,
                DueDate = task.DueDate,
                Status = task.Status.ToString()
            };
        }
        public async Task<TaskDto?> GetTaskDetailsAsync(Guid taskId, Guid userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) return null;

            // التحقق من صلاحية المستخدم
            if (task.CreatorId != userId && task.AssigneeId != userId)
                throw new UnauthorizedAccessException("ليس لديك صلاحية الوصول إلى هذه المهمة");

            return _mapper.Map<TaskDto>(task);
        }

        public async Task<IEnumerable<TaskDto>> GetUserTasksAsync(Guid userId)
        {
            var tasks = await _taskRepository.GetByUserAsync(userId);
            return _mapper.Map<IEnumerable<TaskDto>>(tasks);
        }


        public async Task<bool> UpdateTaskAsync(Guid taskId, UpdateTaskDto dto, Guid userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) return false;

            // التحقق من صلاحية التعديل
            if (task.CreatorId != userId)
                throw new UnauthorizedAccessException("فقط منشئ المهمة يمكنه تعديلها");

            // تحديث الحقول المطلوبة فقط
            if (!string.IsNullOrEmpty(dto.Title))
                task.Title = dto.Title;

            if (!string.IsNullOrEmpty(dto.Description))
                task.Description = dto.Description;

            if (dto.DueDate.HasValue)
                task.DueDate = dto.DueDate.Value;

            if (dto.Priority.HasValue)
                task.Priority = dto.Priority.Value;

            task.LastModified = DateTime.UtcNow;

            return await _taskRepository.UpdateAsync(task);
        }



        public async Task<bool> DeleteTaskAsync(Guid taskId, Guid userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) return false;

            // التحقق من صلاحية الحذف
            if (task.CreatorId != userId)
                throw new UnauthorizedAccessException("فقط منشئ المهمة يمكنه حذفها");

            return await _taskRepository.DeleteAsync(taskId);
        }

        public async Task<bool> AssignTaskAsync(Guid taskId, Guid assigneeId, Guid currentUserId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) return false;

            // التحقق من صلاحية التعديل
            if (task.CreatorId != currentUserId)
                throw new UnauthorizedAccessException("فقط منشئ المهمة يمكنه تعيين المهام");

            // التحقق من وجود المستخدم المسند إليه
            if (!await _userRepository.ExistsAsync(u => u.Id == assigneeId))
                throw new KeyNotFoundException("المستخدم المسند إليه غير موجود");

            task.AssigneeId = assigneeId;
            task.LastModified = DateTime.UtcNow;

            return await _taskRepository.UpdateAsync(task);
        }

        public async Task<bool> ChangeTaskStatusAsync(Guid taskId, TaskStatus status, Guid userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null) return false;

            // التحقق من الصلاحية (المنشئ أو المسند إليه فقط)
            if (task.CreatorId != userId && task.AssigneeId != userId)
                throw new UnauthorizedAccessException("ليس لديك صلاحية تغيير حالة هذه المهمة");

            task.Status = status;
            task.LastModified = DateTime.UtcNow;

            if (status == Core.Entities.TaskStatus.Done)
                task.CompletedAt = DateTime.UtcNow;
            else
                task.CompletedAt = null;

            return await _taskRepository.UpdateAsync(task);
        }

       
    }
}
