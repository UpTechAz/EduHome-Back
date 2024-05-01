using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApplication2.DAL;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public EventController(AppDbContext context, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _context.Events.ToListAsync();
            return View(events);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Event @event)
        {
            if (!ModelState.IsValid) return View(@event);

            if (@event.Photo != null)
            {
                if (!_fileService.IsImage(@event.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(@event);
                }

                int maxSize = 180;
                if (!_fileService.CheckSize(@event.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(@event);
                }

                try
                {
                    var filename = await _fileService.UploadAsync(@event.Photo);
                    @event.FilePath = filename;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while uploading the file.");
                 
                    return View(@event);
                }
            }

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();
            return View(@event);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Event @event)
        {
            if (!ModelState.IsValid) return View(@event);

     
            var existingEvent = await _context.Events.FindAsync(@event.Id);
            if (existingEvent == null) return NotFound();

         
            existingEvent.Title = @event.Title;
            existingEvent.Description = @event.Description;
            existingEvent.Date = @event.Date;
            existingEvent.StartTime = @event.StartTime;
            existingEvent.EndTime = @event.EndTime;
            existingEvent.Venue = @event.Venue;

          
            if (@event.Photo != null)
            {
             
                if (!string.IsNullOrEmpty(existingEvent.FilePath))
                {
                    _fileService.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", existingEvent.FilePath));
                }

        
                int maxSize = 180; 
                if (!_fileService.IsImage(@event.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(@event);
                }
                if (!_fileService.CheckSize(@event.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(@event);
                }

                try
                {
                    var filename = await _fileService.UploadAsync(@event.Photo);
                    existingEvent.FilePath = filename;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while uploading the file.");
              
                    return View(@event);
                }
            }

      
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null) return NotFound();

            if (!string.IsNullOrEmpty(@event.FilePath))
            {
                _fileService.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", @event.FilePath));
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
