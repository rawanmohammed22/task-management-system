using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Core.Interfaces;
using Core.Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{


    

    public class UserRepository : IUserRepository 

    {

        private readonly TaskTrackerDbContext _context; 
        private readonly UserManager<ApplicationUser> _userManager;


        public UserRepository(
        TaskTrackerDbContext context,
        UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ApplicationUser> GetByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.UserProjects)
                .FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task UpdateAsync(ApplicationUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                // Handle related data first
                var userProjects = await _context.UserProjects
                    .Where(up => up.UserId == userId)
                    .ToListAsync();

                _context.UserProjects.RemoveRange(userProjects);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ApplicationUser>> FindAsync(
       Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _context.Users
                .Where(predicate)
                .ToListAsync();
        }


        public async Task<bool> ExistsAsync(Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await _context.Users.AnyAsync(predicate);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersByProjectAsync(Guid projectId)
        {
            return await _context.UserProjects
                .Where(up => up.ProjectId == projectId)
                .Include(up => up.User)
                .Select(up => up.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersWithTasksAsync()
        {
            return await _context.Users
                .Include(u => u.CreatedTasks)
                .Include(u => u.AssignedTasks)
                .Where(u => u.CreatedTasks.Any() || u.AssignedTasks.Any())
                .ToListAsync();
        }

        public async Task UpdateLastLoginAsync(Guid userId)
        {
            var user = await GetByIdAsync(userId);
            if (user != null)
            {
                user.LastLogin = DateTime.UtcNow;
                await UpdateAsync(user);
            }
        }

        public async Task<bool> VerifyPasswordAsync(Guid userId, string password)
        {
            var user = await GetByIdAsync(userId);
            return await _userManager.CheckPasswordAsync(user, password);
        }





        public async Task AddAsync(ApplicationUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }






    }
















}
