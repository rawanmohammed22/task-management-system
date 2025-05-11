using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly TaskTrackerDbContext _context;

        public CommentRepository(TaskTrackerDbContext context)
        {
            _context = context;
        }

        // إضافة Comment جديد
        public async Task<Comment> AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        // استرجاع Comment بواسطة الـ Id
        public async Task<Comment> GetByIdAsync(Guid id)
        {
            return await _context.Comments
                .Include(c => c.Task)  
                .Include(c => c.Author) 
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // استرجاع جميع Comments المرتبطة بـ Task معين
        public async Task<IEnumerable<Comment>> GetByTaskIdAsync(Guid taskId)
        {
            return await _context.Comments
                .Where(c => c.TaskId == taskId)
                .Include(c => c.Author) // تحميل الـ Author المرتبط
                .ToListAsync();
        }

        // استرجاع جميع Comments المرتبطة بـ User معين
        public async Task<IEnumerable<Comment>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Comments
                .Where(c => c.AuthorId == userId)
                .ToListAsync();
        }

        // تحديث Comment
        public async Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        // حذف Comment
        public async Task DeleteAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }

}
