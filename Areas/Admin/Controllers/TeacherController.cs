using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using WebApplication2.DAL;
using WebApplication2.Helpers;
using WebApplication2.Interfaces;
using WebApplication2.Migrations;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using WebApplication2.ViewModels.Teachers;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
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

            return View(PageNatedList<Teacher>.Create(queries, pageIndex, 5, 5));
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Links = await _dbContext.Teachers.ToListAsync();

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
            teachers.CreatedAt = DateTime.UtcNow.AddHours(4);
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
            var Teacher = await _dbContext.Teachers.Include(x=>x.ContactInformation)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (Teacher == null) return NotFound();
            var model = new Teacher
            {
                FullName = Teacher.FullName,
                FilePath = Teacher.FilePath,
                Experience = Teacher.Experience,
                TeacherAbout = Teacher.TeacherAbout,
                Hobbies = Teacher.Hobbies,
                Faculty = Teacher.Faculty,
                ScientificDegree = Teacher.ScientificDegree,
                ContactInformation = Teacher.ContactInformation,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Teacher Teacher, int id)
        {

            if (id != Teacher.Id) return BadRequest();

            if (!ModelState.IsValid) return View(Teacher);
            Teacher? dbTeacher = await _dbContext.Teachers.FindAsync(id);
            dbTeacher!.FullName = Teacher!.FullName;
            dbTeacher.FilePath = Teacher.FilePath;
            dbTeacher.Experience = Teacher.Experience;
            dbTeacher.TeacherAbout = Teacher.TeacherAbout;
            dbTeacher.Hobbies = Teacher.Hobbies;
            dbTeacher.Faculty = Teacher.Faculty;
            dbTeacher.ScientificDegree = Teacher.ScientificDegree;
            var contactInfo = await _dbContext.ContactInformation.FirstOrDefaultAsync(c=>c.TeacherId==id);
            contactInfo!.Email = Teacher.ContactInformation!.Email;
            contactInfo.Number= Teacher.ContactInformation.Number;
            contactInfo.Skype = Teacher.ContactInformation.Skype;
            //dbTeacher.ContactInformation = Teacher.ContactInformation;
            if (Teacher.Photo != null)
            {
                if (!_fileService.IsImage(Teacher.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(Teacher);
                }
                int maxSize = 30;
                if (!_fileService.CheckSize(Teacher.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} MB");
                    //Teacher.FilePath = dbCustomer.FilePath; submit eleyende shekil silinmesin deye 
                    return View(Teacher);
                }
                if(Teacher.FilePath != null)
                {
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images", dbTeacher.FilePath);
                    _fileService.Delete(path);
                }
                var filename = await _fileService.UploadAsync(Teacher.Photo);
                dbTeacher.FilePath = filename;
            }
            Teacher.ContactInformation.TeacherId = Teacher.Id;
            Teacher.CreatedAt = DateTime.UtcNow.AddHours(4);
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
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images", dbTeacher.FilePath);
            _fileService.Delete(path);
            _dbContext.Teachers.Remove(dbTeacher);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
