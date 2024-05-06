using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Helpers;
using WebApplication2.Interfaces;
using WebApplication2.Migrations;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class CourseController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public CourseController(AppDbContext appDbContext, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            IQueryable<Course> queries = _appDbContext.Courses
                .Include(p => p.CoursFeature);

            return View(PageNatedList<Course>.Create(queries, pageIndex, 3, 5));


        }
   

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _appDbContext.Categories.ToListAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            ViewBag.Categories = await _appDbContext.Categories.ToListAsync();
            if (!ModelState.IsValid) return View(course);
            if (course.Photo != null)
            {
                if (!_fileService.IsImage(course.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(course);
                }
                int maxSize = 1024;
                if (!_fileService.CheckSize(course.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(course);
                }

                var filename = await _fileService.UploadAsync(course.Photo);
                course.FilePath = filename;
            }

            course.CoursFeature.CoursesId = course.Id;
            

            await _appDbContext.Courses.AddAsync(course);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var Course = await _appDbContext.Courses
                .Include(c=>c.CoursFeature)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (Course == null) return NotFound();
            var model = new Course
            {
                Id = id,
                CoursName = Course.CoursName,
                CoursApply = Course.CoursApply,
                Certification = Course.Certification,
                CoursAbout = Course.CoursAbout,
                CategoryId = Course.CategoryId,
                CoursFeature = Course.CoursFeature,

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Course course, int id)
        {

            if (id != course.Id) return BadRequest();

            if (!ModelState.IsValid) return View(course);
            Course? dbCourse = await _appDbContext.Courses.FindAsync(id);
            dbCourse.CoursName = course.CoursName;
            dbCourse.CoursApply = course.CoursApply;
            dbCourse.Certification = course.Certification;
            dbCourse.CoursAbout = course.CoursAbout;
            dbCourse.CategoryId = course.CategoryId;

            var dbCourseFeature = await _appDbContext.CourseFeature.FirstOrDefaultAsync(cf=>cf.CoursesId == id);
            dbCourseFeature.Starts = course.CoursFeature.Starts;
            dbCourseFeature.Duration = course.CoursFeature.Duration;
            dbCourseFeature.ClassDuration = course.CoursFeature.ClassDuration;
            dbCourseFeature.SkillLevel = course.CoursFeature.SkillLevel;
            dbCourseFeature.Language = course.CoursFeature.Language;
            dbCourseFeature.StudentsCount = course.CoursFeature.StudentsCount;
            dbCourseFeature.Assesments = course.CoursFeature.Assesments;
            dbCourseFeature.CourseFee = course.CoursFeature.CourseFee;

            if (course.Photo != null)
            {
                if (!_fileService.IsImage(course.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(course);
                }
                int maxSize = 30;
                if (!_fileService.CheckSize(course.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB");
                     
                    return View(course);
                }
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbCourse.FilePath);
                _fileService.Delete(path);
                var filename = await _fileService.UploadAsync(course.Photo);
                dbCourse.FilePath = filename;
            }
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbCourse = await _appDbContext.Courses.FindAsync(id);
            if (dbCourse == null) return NotFound();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbCourse.FilePath);
            _fileService.Delete(path);
            _appDbContext.Courses.Remove(dbCourse);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
