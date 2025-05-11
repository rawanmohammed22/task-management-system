using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
  public class SubTaskRepository : ISubTaskRepository
    {
        private readonly TaskTrackerDbContext _context;

        public SubTaskRepository(TaskTrackerDbContext context)
        {
            _context = context;
        }

        // إضافة SubTask جديد
        public async Task<SubTask> AddAsync(SubTask subTask)
        {
            await _context.SubTasks.AddAsync(subTask);
            await _context.SaveChangesAsync();
            return subTask;
        }

        // استرجاع SubTask بواسطة الـ Id
        public async Task<SubTask> GetByIdAsync(Guid id)
        {
            return await _context.SubTasks
                .Include(st => st.Task)  // إذا كنت ترغب في تحميل الـ Task المرتبط
                .FirstOrDefaultAsync(st => st.Id == id);
        }

        // استرجاع جميع SubTasks المرتبطة بـ Task معين
        public async Task<IEnumerable<SubTask>> GetByTaskIdAsync(Guid taskId)
        {
            return await _context.SubTasks
                .Where(st => st.TaskId == taskId)
                .ToListAsync();
        }

        // تحديث SubTask
        public async Task UpdateAsync(SubTask subTask)
        {
            _context.SubTasks.Update(subTask);
            await _context.SaveChangesAsync();
        }

        // حذف SubTask
        public async Task DeleteAsync(SubTask subTask)
        {
            _context.SubTasks.Remove(subTask);
            await _context.SaveChangesAsync();
        }
    }
}
