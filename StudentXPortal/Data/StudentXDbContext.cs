using Microsoft.EntityFrameworkCore;
using StudentX.StudentXPortal.Models;

namespace StudentX.StudentXPortal.Data
{

    public class StudentXDbContext : DbContext
    {
        public StudentXDbContext(DbContextOptions<StudentXDbContext> options)
          : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite Key: UserCourse
            modelBuilder.Entity<UserCourse>()
                .HasKey(uc => new { uc.UserId, uc.CourseId });

            base.OnModelCreating(modelBuilder);
        }
    }
}