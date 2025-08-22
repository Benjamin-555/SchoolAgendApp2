using Microsoft.EntityFrameworkCore;
using SchoolAgendCRUD.Domain.Entities;
using SchoolAgendCRUD.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolAgend.Infrastructure.Data.Repositories
{
    public class GradeRepository
    {
        private readonly SchoolAgendCRUDDbContext _context;

        public object Grades { get; set; }
        public object Students { get; set; }

        public GradeRepository(SchoolAgendCRUDDbContext context)
        {
            _context = context;
        }

        public async Task<List<Grade>> GetGradesWithStudents(int studentId)
        {
            return await _context.Grades
                .Include(g => g.Student)
                .Where(g => g.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<List<Grade>> GetGradesByStudentId(int studentId)
        {
            return await _context.Grades.Where(g => g.StudentId == studentId).ToListAsync();
        }

        public async Task<List<Grade>> GetAllGrades()
        {
            return await _context.Grades.ToListAsync();
        }

        public async Task<Grade> GetGradeById(int id)
        {
            return await _context.Grades.FindAsync(id);
        }
        public async Task<Grade> CreateGrade(Grade grade)
        {
            if (grade == null)
            {
                throw new ArgumentNullException(nameof(grade), "Grade cannot be null");
            }
            var studentExists = await _context.Students.AnyAsync(s => s.Id == grade.StudentId);
            if (!studentExists)
            {
                throw new InvalidOperationException($"No student found with ID = {grade.StudentId}");
            }
            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();
            return grade;
        }
        public async Task<Grade> UpdateGrade(Grade grade)
        {
            if (grade == null)
            {
                throw new ArgumentNullException(nameof(grade), "Grade cannot be null");
            }
            var existingGrade = await _context.Grades.FindAsync(grade.Id);
            if (existingGrade == null)
            {
                throw new KeyNotFoundException($"Grade with ID {grade.Id} not found.");
            }
            existingGrade.Value = grade.Value;
            existingGrade.StudentId = grade.StudentId;
            _context.Grades.Update(existingGrade);
            await _context.SaveChangesAsync();
            return existingGrade;
        }
        public async Task DeleteGrade(int id)
        {
            var grade = await _context.Grades.FindAsync(id);
            if (grade == null)
            {
                throw new KeyNotFoundException($"Grade with ID {id} not found.");
            }
            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();
        }
    }
}
