using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    // Core/Interfaces/IUserRepository.cs
    public interface IUserRepository
    {
        Task<ApplicationUser> GetByIdAsync(Guid id);
        Task<ApplicationUser> GetByEmailAsync(string email);
        Task AddAsync(ApplicationUser user);
        Task UpdateAsync(ApplicationUser user);
        Task DeleteAsync(Guid userId);

        // Query Operations
        Task<IEnumerable<ApplicationUser>> FindAsync(Expression<Func<ApplicationUser, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<ApplicationUser, bool>> predicate);

        // Specialized Queries
        Task<IEnumerable<ApplicationUser>> GetUsersByProjectAsync(Guid projectId);
        Task<IEnumerable<ApplicationUser>> GetUsersWithTasksAsync();

        // Authentication Related
        Task UpdateLastLoginAsync(Guid userId);
        Task<bool> VerifyPasswordAsync(Guid userId, string password);
    }

}
