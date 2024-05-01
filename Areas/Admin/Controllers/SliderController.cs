using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WebApplication2.DAL;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public SliderController(AppDbContext context, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.ToListAsync();
            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Slider slider, IFormFile photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null)
                {
                    if (!_fileService.IsImage(photo))
                    {
                        ModelState.AddModelError("Photo", "The file must be in Image format.");
                        return View(slider);
                    }
                    int maxSize = 185;
                    if (!_fileService.CheckSize(photo, maxSize))
                    {
                        ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB");
                        return View(slider);
                    }
                    slider.FilePath = await _fileService.UploadAsync(photo);
                }

                await _context.Sliders.AddAsync(slider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Slider slider, IFormFile photo)
        {
            if (id != slider.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (photo != null)
                    {
                        if (!_fileService.IsImage(photo))
                        {
                            ModelState.AddModelError("Photo", "The file must be in Image format.");
                            return View(slider);
                        }
                        int maxSize = 185;
                        if (!_fileService.CheckSize(photo, maxSize))
                        {
                            ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB");
                            return View(slider);
                        }
                        slider.FilePath = await _fileService.UploadAsync(photo);
                    }

                    _context.Update(slider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SliderExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }

            return View(slider);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (!string.IsNullOrEmpty(slider.FilePath))
            {
                _fileService.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", slider.FilePath));
            }
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SliderExists(int id)
        {
            return _context.Sliders.Any(e => e.Id == id);
        }
    }
}
