using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CourseController(AppDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<IActionResult> Index()
        {
            var courses = await _dbContext.Courses.ToListAsync(); 
            return View(courses);
        }
        public async Task<IActionResult> Details(Course? Course)
        {
            Course = await _dbContext.Courses
                .Include(x => x.CoursFeature)
                .Include(x => x.Category)
                .Include(x => x.CourseComment)
                .FirstOrDefaultAsync();
            Course = new Course
            {
                FilePath = Course.FilePath,
                CoursName = Course.CoursName,
                CoursFeature = Course.CoursFeature,
                CoursAbout = Course.CoursAbout,
                CoursApply = Course.CoursApply,
                Certification = Course.Certification,
                Category = Course.Category,
                CourseComment = Course.CourseComment,

            };
            return View(Course);
        }

   

    }
}