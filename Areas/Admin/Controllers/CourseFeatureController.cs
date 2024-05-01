using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CourseFeatureController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CourseFeatureController(AppDbContext appDbContext )
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            List<CourseFeature> courseFeatures = await _appDbContext.CourseFeature.Include(x => x.Courses).ToListAsync();
            ViewBag.CourseFeature = courseFeatures.Count;
            return View(courseFeatures);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CourseFeature courseFeature)
        {
            if (!ModelState.IsValid) return View(courseFeature);
            await _appDbContext.CourseFeature.AddAsync(courseFeature);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var courseFeature = await _appDbContext.CourseFeature.
                FirstOrDefaultAsync(c => c.Id == id);
            if (courseFeature == null) return NotFound();

            return View(courseFeature);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseFeature courseFeature, int id)
        {
            if (id != courseFeature.Id) return BadRequest();
            if (!ModelState.IsValid) return View(courseFeature);
            var dbCourseFeature = await _appDbContext.CourseFeature.FindAsync(id);
            dbCourseFeature.Starts = courseFeature.Starts;
            dbCourseFeature.Duration = courseFeature.Duration;
            dbCourseFeature.ClassDuration = courseFeature.ClassDuration;
            dbCourseFeature.SkillLevel = courseFeature.SkillLevel;
            dbCourseFeature.Language = courseFeature.Language;
            dbCourseFeature.StudentsCount = courseFeature.StudentsCount;
            dbCourseFeature.Assesments = courseFeature.Assesments;
            dbCourseFeature.CourseFee = courseFeature.CourseFee;

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbContactinfo = await _appDbContext.ContactInformation.FindAsync(id);
            if (dbContactinfo == null) return BadRequest();
            _appDbContext.ContactInformation.Remove(dbContactinfo);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
