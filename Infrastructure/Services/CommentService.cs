using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CommentService
    {
        private readonly TaskTrackerDbContext _context;

        public CommentService(TaskTrackerDbContext context)
        {
            _context = context;
        }

        // إضافة تعليق جديد
        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        // الحصول على تعليقات بناءً على TaskId
        public async Task<List<Comment>> GetCommentsByTaskIdAsync(Guid taskId)
        {
            return await _context.Comments
                .Where(c => c.TaskId == taskId)
                .ToListAsync();
        }

        // حذف تعليق
        public async Task<bool> DeleteCommentAsync(Guid commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null) return false;

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
