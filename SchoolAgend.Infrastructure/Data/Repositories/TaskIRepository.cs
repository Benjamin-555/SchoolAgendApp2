using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolAgend.Application.Contracts;
using SchoolAgend.Domain.Entities;
using SchoolAgendCRUD.Infrastructure.Data;

namespace SchoolAgend.Infrastructure.Data.Repositories
{
    public class TaskIRepository : ITaskIRepository
    {
        private readonly SchoolAgendCRUDDbContext _context;
        public TaskIRepository(SchoolAgendCRUDDbContext context) => _context = context;

        public async Task<IEnumerable<TaskI>> GetAllAsync() =>
            await _context.TasksI.ToListAsync();

        public async Task<TaskI> GetByIdAsync(int id) =>
            await _context.TasksI.FindAsync(id);

        public async Task AddAsync(TaskI task)
        {
            _context.TasksI.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskI task)
        {
            _context.TasksI.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TaskI task)
        {
            _context.TasksI.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task GetTaskById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
