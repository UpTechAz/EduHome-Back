using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Helpers;
using WebApplication2.Interfaces;
using WebApplication2.Migrations;
using WebApplication2.Models;

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
        public async Task<IActionResult> Index()
        {
            List<Course>? courses = await _appDbContext.Courses.ToListAsync();
                //.Include(x => x.CourseFeature);

            return View(courses);
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Course course)
        {
            if (!ModelState.IsValid) return View(course);
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
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(course);
                }

                var filename = await _fileService.UploadAsync(course.Photo);
                course.FilePath = filename;
            }
            course.CoursFeature.CoursesId = course.Id;
            course.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _appDbContext.Courses.AddAsync(course);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Update 
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Course? course = await _appDbContext.Courses
                .Include(c => c.CoursFeature)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null) return BadRequest();
            var model = new Course
            {
                Id = id,
                CoursName = course.CoursName,
                CoursAbout = course.CoursAbout,
                CoursApply = course.CoursApply,
                CourseFeaturedId = course.CourseFeaturedId,
                Certification = course.Certification,
                FilePath = course.FilePath,
                CategoryId = course.CategoryId,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Course course)
        {
            if (id != course.Id) return BadRequest();
            if (!ModelState.IsValid) return View(course);
            Course? dbCourse = await _appDbContext.Courses.FindAsync(id);
            dbCourse.CoursName = course.CoursName;
            dbCourse.CategoryId = course.CategoryId;
            dbCourse.CoursAbout = course.CoursAbout;
            dbCourse.CoursApply = course.CoursApply;
            dbCourse.FilePath = course.FilePath;
            dbCourse.Certification = course.Certification;
            dbCourse.CourseFeaturedId = course.CourseFeaturedId;
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
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} MB");
                    //Teacher.FilePath = dbCustomer.FilePath; submit eleyende shekil silinmesin deye 
                    return View(course);
                }
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbCourse.FilePath);
                _fileService.Delete(path);
                var filename = await _fileService.UploadAsync(course.Photo);
                dbCourse.FilePath = filename;
            }
            course.CoursFeature.CoursesId = course.Id;
            course.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _appDbContext.Teachers.FindAsync(id);
            if (course == null) return NotFound();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", course.FilePath);
            _fileService.Delete(path);
            _appDbContext.Teachers.Remove(course);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
