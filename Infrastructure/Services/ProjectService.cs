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
    public class ProjectService
    {
        private readonly TaskTrackerDbContext _context;

        public ProjectService(TaskTrackerDbContext context)
        {
            _context = context;
        }

        // إضافة مشروع
        public async Task<Project> AddProjectAsync(Project project)
        {
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project;
        }

        // الحصول على كل المشاريع
        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects.ToListAsync();
        }

        // الحصول على مشروع بواسطة ID
        public async Task<Project> GetProjectByIdAsync(Guid projectId)
        {
            return await _context.Projects.FindAsync(projectId);
        }

        // تعديل مشروع
        public async Task<Project> UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
            return project;
        }

        // حذف مشروع
        public async Task<bool> DeleteProjectAsync(Guid projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
