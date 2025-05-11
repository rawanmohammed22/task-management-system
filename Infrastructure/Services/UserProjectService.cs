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
    public class UserProjectService
    {
        private readonly TaskTrackerDbContext _context;

        public UserProjectService(TaskTrackerDbContext context)
        {
            _context = context;
        }

        // إضافة عضو إلى المشروع
        public async Task<UserProject> AddUserToProjectAsync(UserProject userProject)
        {
            _context.UserProjects.Add(userProject);
            await _context.SaveChangesAsync();
            return userProject;
        }

        // الحصول على أعضاء المشروع
        public async Task<List<UserProject>> GetUsersByProjectIdAsync(Guid projectId)
        {
            return await _context.UserProjects
                .Where(up => up.ProjectId == projectId)
                .ToListAsync();
        }

        // إزالة عضو من المشروع
        public async Task<bool> RemoveUserFromProjectAsync(Guid userId, Guid projectId)
        {
            var userProject = await _context.UserProjects
                .FirstOrDefaultAsync(up => up.UserId == userId && up.ProjectId == projectId);

            if (userProject == null) return false;

            _context.UserProjects.Remove(userProject);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
