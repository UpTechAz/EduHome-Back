using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using WebApplication2.DAL;
using WebApplication2.Helpers;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class StaticFileController : Controller
    {
        private readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public StaticFileController(AppDbContext appDbContext , IWebHostEnvironment webHostEnvironment , IFileService fileService)
        {
            _appDbContext = appDbContext;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _appDbContext.StaticFiles.ToListAsync();

            return View(model); 
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(StaticFile model)
        {
            if (!ModelState.IsValid) return View(model);
            if (model.HeaderLogoFile != null)
            {
                if (!_fileService.IsImage(model.HeaderLogoFile))
                {
                    ModelState.AddModelError("HeaderLogoFile", "The file must be in Image format.");
                    return View(model);
                }
                int maxSize = 1024;
                if (!_fileService.CheckSize(model.HeaderLogoFile, maxSize))
                {
                    ModelState.AddModelError("HeaderLogoFile", $"The size of the image should not exceed {maxSize} KB.");
                    return View(model);
                }

                var filename = await _fileService.UploadAsync(model.HeaderLogoFile);
                model.HeaderLogo = filename;
            }
            await _appDbContext.StaticFiles.AddAsync(model);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var Image = await _appDbContext.StaticFiles.FindAsync(id);
            if (Image == null)
            {
                return BadRequest();
            }
            var model = new StaticFile
            {
                Id = Image.Id,
                HeaderLogo = Image.HeaderLogo,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(StaticFile model, int id)
        {

            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            var dbImage = await _appDbContext.StaticFiles.FindAsync(id);
            dbImage.HeaderLogo = model.HeaderLogo;


            if (model.HeaderLogoFile != null)
            {

                if (!_fileService.IsImage(model.HeaderLogoFile))
                {
                    ModelState.AddModelError("HeaderLogoFile", "The file must be in Image format.");
                    return View(model);
                }
                int maxSize = 1024;
                if (!_fileService.CheckSize(model.HeaderLogoFile, maxSize))
                {
                    ModelState.AddModelError("HeaderLogoFile", $"The size of the image should not exceed {maxSize} KB");
                    return View(model);
                }
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img"); 
                _fileService.Delete(path);
                var filename = await _fileService.UploadAsync(model.HeaderLogoFile);
                dbImage.HeaderLogo = filename;
            }

            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbImage = await _appDbContext.StaticFiles.FindAsync(id);
            if (dbImage == null) return NotFound();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbImage.HeaderLogo);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _appDbContext.StaticFiles.Remove(dbImage);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
