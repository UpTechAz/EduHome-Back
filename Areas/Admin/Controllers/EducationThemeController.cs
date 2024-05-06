using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class EducationThemeController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EducationThemeController(AppDbContext dbContext, IFileService fileService,
            IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            EducationTheme? eduTheme = await _dbContext.EducationTheme.FirstOrDefaultAsync();
            return View(eduTheme);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EducationTheme eduTheme)
        {
            if (!ModelState.IsValid) return NotFound();

            if (eduTheme.Photo != null)
            {
                if (!_fileService.IsImage(eduTheme.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(eduTheme);
                }
                int maxSize = 1024;
                if (!_fileService.CheckSize(eduTheme.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(eduTheme);
                }
                var filename = await _fileService.UploadAsync(eduTheme.Photo);
                eduTheme.FilePath = filename;
            }
            await _dbContext.EducationTheme.AddAsync(eduTheme);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var eduTheme = await _dbContext.EducationTheme.FirstOrDefaultAsync(l => l.Id == id);
            if (eduTheme == null) return BadRequest();
            var model = new EducationTheme
            {
                Title = eduTheme.Title,
                Description = eduTheme.Description,
                FilePath = eduTheme.FilePath,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(EducationTheme eduTheme, int id)
        {
            if (id != eduTheme.Id) return BadRequest();
            if (!ModelState.IsValid) return View(eduTheme);
            var dbTheme = await _dbContext.EducationTheme.FindAsync(id);
            dbTheme.Title = eduTheme.Title;

            var filename = await _fileService.UploadAsync(eduTheme.Photo);
            dbTheme.FilePath = filename;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var dblink = await _dbContext.EducationTheme.FindAsync(id);
            if (dblink == null) return BadRequest();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/images", dblink.FilePath);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _dbContext.EducationTheme.Remove(dblink);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
