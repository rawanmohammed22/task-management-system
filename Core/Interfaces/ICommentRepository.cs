using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    
    
        public interface ICommentRepository
        {
            Task<Comment> AddAsync(Comment comment);
            Task<Comment> GetByIdAsync(Guid id);
            Task<IEnumerable<Comment>> GetByTaskIdAsync(Guid taskId);
            Task<IEnumerable<Comment>> GetByUserIdAsync(Guid userId);
            Task UpdateAsync(Comment comment);
            Task DeleteAsync(Comment comment);
        }
    
}
