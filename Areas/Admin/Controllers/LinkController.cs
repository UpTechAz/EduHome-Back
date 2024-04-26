using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Pkcs;
using WebApplication2.DAL;
using WebApplication2.Helpers;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="SuperAdmin")]

    public class LinkController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LinkController(AppDbContext dbContext, IFileService fileService, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _fileService = fileService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<Link> linkInfos = await _dbContext.Links.ToListAsync();
            ViewBag.LinkInfo = linkInfos.Count;
            return View(linkInfos);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Link links)
        {
            if (!ModelState.IsValid) { return NotFound(); }
            bool isExist = await _dbContext.Links
            .AnyAsync(x => x.Name.ToLower().Trim() == links.Name.ToLower().Trim());
            if (links.Photo != null)
            {
                if (!_fileService.IsImage(links.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(links);
                }
                int maxSize = 30;
                if (!_fileService.CheckSize(links.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(links);
                }
                var filename = await _fileService.UploadAsync(links.Photo);
                links.Icon = filename;
            }
            await _dbContext.Links.AddAsync(links);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var link = await _dbContext.Links.FirstOrDefaultAsync(l => l.Id == id);
            if (link == null) return BadRequest();
            var model = new Link
            {
                Id = id,
                Name = link.Name,
                Icon = link.Icon,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Link link, int id)
        {
            if(id != link.Id) return BadRequest(); 
            if(!ModelState.IsValid) return View(link);
            var dblink = await _dbContext.Links.FindAsync(id);
            dblink.Name = link.Name;

            var filename = await _fileService.UploadAsync(link.Photo);
            dblink.Icon = filename;

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var dblink = await _dbContext.Links.FindAsync(id);
            if(dblink == null) return BadRequest();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dblink.Icon);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _dbContext.Links.Remove(dblink);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
