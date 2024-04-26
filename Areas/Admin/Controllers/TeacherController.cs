using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApplication2.DAL;
using WebApplication2.Helpers;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class TeacherController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public TeacherController(AppDbContext appDbContext,
            IWebHostEnvironment webHostEnvironment,
            IFileService fileService)
        {
            _dbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index(int pageIndex = 1)
        {
            IQueryable<Teacher> queries = _dbContext.Teachers
                .Include(p => p.ContactInformation);
            //.Where(p => p.IsDeleted == false);

            return View(PageNatedList<Teacher>.Create(queries, pageIndex, 3, 5));


        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Links = await _dbContext.Links.ToListAsync();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Teacher teachers)
        {
            if (!ModelState.IsValid) return View(teachers);
            if (teachers.Photo != null)
            {
                if (!_fileService.IsImage(teachers.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(teachers);
                }
                int maxSize = 30;
                if (!_fileService.CheckSize(teachers.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(teachers);
                }

                var filename = await _fileService.UploadAsync(teachers.Photo);
                teachers.FilePath = filename;
            }

            teachers.ContactInformation.TeacherId = teachers.Id;
            teachers.TeacherLinks = new List<TeacherLink>();
            if (teachers.TeacherLinks is not null && teachers.TeacherLinks.Count() > 0)
            {
                foreach (var item in teachers.TeacherLinks)
                {
                    item.TeacherId = teachers.Id;
                }
            }

            await _dbContext.Teachers.AddAsync(teachers);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var Teacher = await _dbContext.Teachers
                .FirstOrDefaultAsync(c => c.Id == id);
            if (Teacher == null) return NotFound();
            var model = new Teacher
            {
                Id = id,
                FullName = Teacher.FullName,
                FilePath = Teacher.FilePath,
                Experience = Teacher.Experience,
                TeacherAbout = Teacher.TeacherAbout,
                Hobbies = Teacher.Hobbies,
                Faculty = Teacher.Faculty,
                ScientificDegree = Teacher.ScientificDegree,


            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Teacher Teachers, int id)
        {

            if (id != Teachers.Id) return BadRequest();

            if (!ModelState.IsValid) return View(Teachers);
            Teacher? dbTeacher = await _dbContext.Teachers.FindAsync(id);
            dbTeacher.FullName = Teachers.FullName;
            dbTeacher.FilePath = Teachers.FilePath;
            dbTeacher.Experience = Teachers.Experience;
            dbTeacher.TeacherAbout = Teachers.TeacherAbout;
            dbTeacher.Hobbies = Teachers.Hobbies;
            dbTeacher.Faculty = Teachers.Faculty;
            dbTeacher.ScientificDegree = Teachers.ScientificDegree;

            if (Teachers.Photo != null)
            {
                if (!_fileService.IsImage(Teachers.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(Teachers);
                }
                int maxSize = 30;
                if (!_fileService.CheckSize(Teachers.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} MB");
                    //Teacher.FilePath = dbCustomer.FilePath; submit eleyende shekil silinmesin deye 
                    return View(Teachers);
                }
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbTeacher.FilePath);
                _fileService.Delete(path);
                var filename = await _fileService.UploadAsync(Teachers.Photo);
                dbTeacher.FilePath = filename;
            }
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbTeacher = await _dbContext.Teachers.FindAsync(id);
            if (dbTeacher == null) return NotFound();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbTeacher.FilePath);
            _fileService.Delete(path);
            _dbContext.Teachers.Remove(dbTeacher);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
