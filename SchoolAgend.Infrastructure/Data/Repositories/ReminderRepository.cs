using SchoolAgend.Domain.Entities;
using SchoolAgend.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolAgend.Application.Contracts;
using SchoolAgendCRUD.Infrastructure.Data;

namespace SchoolAgend.Infrastructure.Data.Repositories
{
    public class ReminderRepository : IReminderRepository
    {
        private readonly SchoolAgendCRUDDbContext _context;
        public ReminderRepository(SchoolAgendCRUDDbContext context) => _context = context;

        public async Task<IEnumerable<Reminder>> GetAllAsync() =>
            await _context.Reminders.ToListAsync();

        public async Task<Reminder> GetByIdAsync(int id) =>
            await _context.Reminders.FindAsync(id);

        public async Task AddAsync(Reminder reminder)
        {
            _context.Reminders.Add(reminder);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reminder reminder)
        {
            _context.Reminders.Update(reminder);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Reminder reminder)
        {
            _context.Reminders.Remove(reminder);
            await _context.SaveChangesAsync();
        }
    }
}
