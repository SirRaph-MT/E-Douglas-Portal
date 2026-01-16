using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Core.DB
{
    public class AppDBContex : IdentityDbContext
    {
        public AppDBContex(DbContextOptions<AppDBContex>options): base(options)
        {
            
        }
        public DbSet<ApplicationUser> applicationUsers { get; set; }
        public DbSet<CourseTopic> CourseTopics { get; set; }
        public DbSet<CourseSubTopic> CourseSubTopics { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentProfile> StudentProfiles { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<ProgressRecord> ProgressRecords { get; set; }

    }
}
