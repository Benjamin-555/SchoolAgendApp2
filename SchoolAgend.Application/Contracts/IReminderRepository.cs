using SchoolAgend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolAgend.Application.Contracts;

namespace SchoolAgend.Application.Contracts
{
    public interface IReminderRepository
    {
        Task<IEnumerable<Reminder>> GetAllAsync();
        Task<Reminder> GetByIdAsync(int id);
        Task AddAsync(Reminder reminder);
        Task UpdateAsync(Reminder reminder);
        Task DeleteAsync(Reminder reminder);
    }
}
