using SchoolAgend.Domain.Entities;
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
    public class SessionRepository : ISessionRepository
    {
        private readonly SchoolAgendCRUDDbContext _context;
        public SessionRepository(SchoolAgendCRUDDbContext context) => _context = context;

        public async Task<IEnumerable<Session>> GetAllAsync() =>
            await _context.Sessions.ToListAsync();

        public async Task<Session> GetByIdAsync(int id) =>
            await _context.Sessions.FindAsync(id);

        public async Task AddAsync(Session session)
        {
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Session session)
        {
            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Session session)
        {
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }
    }

}
