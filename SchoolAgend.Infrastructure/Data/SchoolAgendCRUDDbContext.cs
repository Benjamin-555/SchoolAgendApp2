using Microsoft.EntityFrameworkCore;
using SchoolAgend.Domain.Entities;
using TaskI = SchoolAgend.Domain.Entities.TaskI;

namespace SchoolAgendCRUD.Infrastructure.Data
{
    public class SchoolAgendCRUDDbContext : DbContext
    {
        private string _connectionString;

        public SchoolAgendCRUDDbContext(DbContextOptions<SchoolAgendCRUDDbContext> options): base(options) 
        { 
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().Ignore(c => c.RowVersion);
            modelBuilder.Entity<Reminder>().Ignore(r => r.RowVersion);
            modelBuilder.Entity<Session>().Ignore(s => s.RowVersion);
            modelBuilder.Entity<TaskI>().Ignore(t => t.RowVersion);
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<TaskI> TasksI { get; set; }
    }
}
