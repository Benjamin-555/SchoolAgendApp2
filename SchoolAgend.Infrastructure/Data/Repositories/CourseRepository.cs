using SchoolAgend.Domain.Entities;
using SchoolAgendCRUD.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SchoolAgend.Application.Contracts;

namespace SchoolAgend.Infrastructure.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly SchoolAgendCRUDDbContext _context;
        public CourseRepository(SchoolAgendCRUDDbContext context) => _context = context;

        public async Task<IEnumerable<Course>> GetAllAsync() =>
            await _context.Courses.ToListAsync();

        public async Task<Course> GetByIdAsync(int id) =>
            await _context.Courses.FindAsync(id);

        public async Task AddAsync(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Course course)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }
    }
}
