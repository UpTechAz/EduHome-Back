
// SpeakerController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.DAL;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SpeakerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public SpeakerController(AppDbContext context, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var speakers = await _context.Speakers.ToListAsync();
            return View(speakers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Speaker speaker)
        {
            if (!ModelState.IsValid)
            {
                return View(speaker);
            }

            if (speaker.Photo != null)
            {
                if (!_fileService.IsImage(speaker.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(speaker);
                }

                int maxSize = 30; 
                if (!_fileService.CheckSize(speaker.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(speaker);
                }

                try
                {
                    var filename = await _fileService.UploadAsync(speaker.Photo);
                    speaker.FilePath = filename;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while uploading the file.");
                
                    return View(speaker);
                }
            }

            _context.Speakers.Add(speaker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var speaker = await _context.Speakers.FindAsync(id);
            if (speaker == null)
            {
                return NotFound();
            }
            return View(speaker);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Speaker speaker)
        {
            if (!ModelState.IsValid)
            {
                return View(speaker);
            }

            var existingSpeaker = await _context.Speakers.FindAsync(speaker.Id);
            if (existingSpeaker == null)
            {
                return NotFound();
            }

            existingSpeaker.FullName = speaker.FullName;
            existingSpeaker.WorkPlace = speaker.WorkPlace;
            existingSpeaker.Profession = speaker.Profession;

            if (speaker.Photo != null)
            {
                if (!_fileService.IsImage(speaker.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(speaker);
                }

                int maxSize = 30; 
                if (!_fileService.CheckSize(speaker.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(speaker);
                }

                try
                {
                    if (!string.IsNullOrEmpty(existingSpeaker.FilePath))
                    {
                        _fileService.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", existingSpeaker.FilePath));
                    }

                    var filename = await _fileService.UploadAsync(speaker.Photo);
                    existingSpeaker.FilePath = filename;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while uploading the file.");
                 
                    return View(speaker);
                }
            }

            _context.Entry(existingSpeaker).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var speaker = await _context.Speakers.FindAsync(id);
            if (speaker == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(speaker.FilePath))
            {
                _fileService.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", speaker.FilePath));
            }

            _context.Speakers.Remove(speaker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
