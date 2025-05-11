using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class SubTaskService : ISubTaskService
    {
        private readonly ISubTaskRepository _subTaskRepository;

        public SubTaskService(ISubTaskRepository subTaskRepository)
        {
            _subTaskRepository = subTaskRepository;
        }

        public async Task<SubTask> CreateSubTaskAsync(string title, string description, Guid taskId)
        {
            var subTask = new SubTask
            {
                Title = title,
                Description = description,
                TaskId = taskId
            };

            await _subTaskRepository.AddAsync(subTask);
            return subTask;
        }

        public async Task<SubTask> GetSubTaskByIdAsync(Guid id)
        {
            return await _subTaskRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<SubTask>> GetSubTasksByTaskIdAsync(Guid taskId)
        {
            return await _subTaskRepository.GetByTaskIdAsync(taskId);
        }

        public async Task<bool> UpdateSubTaskAsync(Guid id, string title, string description, bool isCompleted)
        {
            var subTask = await _subTaskRepository.GetByIdAsync(id);
            if (subTask == null) return false;

            subTask.Title = title;
            subTask.Description = description;
            subTask.IsCompleted = isCompleted;

            await _subTaskRepository.UpdateAsync(subTask);
            return true;
        }

        public async Task<bool> DeleteSubTaskAsync(Guid id)
        {
            var subTask = await _subTaskRepository.GetByIdAsync(id);
            if (subTask == null) return false;

            await _subTaskRepository.DeleteAsync(subTask);
            return true;
        }
    }

}
