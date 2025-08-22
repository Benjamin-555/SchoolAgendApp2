using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolAgend.Domain.Entities;
using SchoolAgend.Application.Contracts;

namespace SchoolAgend.Application.Contracts
{
    public interface ITaskIRepository
    {
        Task<IEnumerable<TaskI>> GetAllAsync();
        Task<TaskI> GetByIdAsync(int id);
        Task AddAsync(TaskI task);
        Task UpdateAsync(TaskI task);
        Task DeleteAsync(TaskI task);
    }
}
