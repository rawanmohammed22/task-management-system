using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskStatus = Core.Entities.TaskStatus;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
   public class TaskRepository : ITaskRepository
    {
        private readonly TaskTrackerDbContext _context;

        public TaskRepository(TaskTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .Include(t => t.Creator)
                .Include(t => t.Assignee)
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetByUserAsync(Guid userId)
        {
            return await _context.Tasks
                .Where(t => t.CreatorId == userId || t.AssigneeId == userId)
                .Include(t => t.Project)
                .OrderByDescending(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = await GetByIdAsync(id);
            if (task == null) return false;

            _context.Tasks.Remove(task);
            return await _context.SaveChangesAsync() > 0;
        }

       
    }

}

