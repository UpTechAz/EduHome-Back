using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using WebApplication2.Models;

namespace WebApplication2.DAL
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            :base(options) 
        {
       
        }
        public DbSet <Course> Courses { get; set; }
        public DbSet <CourseTour> CoursTour { get; set; }
        public DbSet <CourseFeature> CourseFeature { get; set; }
        public DbSet <Category> Categories { get; set; }
        public DbSet <ContactInformation> ContactInformation { get; set; }
        public DbSet <EducationTheme> EducationTheme { get; set; }
        public DbSet< Event> Events { get; set; }   
        public DbSet <Link> Links { get; set; }
        public DbSet <NoticeBoard> NoticesBoards { get; set; }
        public DbSet <Message> Messages { get; set; }
        public DbSet <Skill> Skills { get; set; }
        public DbSet <Speaker> Speakers { get; set; }
        public DbSet <Slider> Sliders { get; set; }
        public DbSet <StudentQuote> StudentComments { get; set; }
        public DbSet <Teacher> Teachers { get; set; }
        public DbSet <TeacherLink> TeachersLink { get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Speaker)
                .WithMany()
                .HasForeignKey(e => e.SpeakerId);
            base.OnModelCreating(modelBuilder);
        }
    }

}
