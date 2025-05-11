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
    public class AttachService
    {
        private readonly TaskTrackerDbContext _context;

        public AttachService(TaskTrackerDbContext context)
        {
            _context = context;
        }

        // إضافة مرفق
        public async Task<Attacht> AddAttachmentAsync(Attacht attachment)
        {
            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();
            return attachment;
        }

        // الحصول على مرفقات بناءً على TaskId
        public async Task<List<Attacht>> GetAttachmentsByTaskIdAsync(Guid taskId)
        {
            return await _context.Attachments
                .Where(a => a.TaskId == taskId)
                .ToListAsync();
        }

        // حذف مرفق
        public async Task<bool> DeleteAttachmentAsync(Guid attachmentId)
        {
            var attachment = await _context.Attachments.FindAsync(attachmentId);
            if (attachment == null) return false;

            _context.Attachments.Remove(attachment);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
