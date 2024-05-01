using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class TagController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public TagController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _appDbContext.Tags.ToListAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }

            bool isExist = await _appDbContext.Tags.AnyAsync(t => t.Name == tag.Name);

            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda Tag movcuddur");
                return View(tag);
            }

            tag.CreatedAt = DateTime.Now;
            await _appDbContext.Tags.AddAsync(tag);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var dbTag = await _appDbContext.Tags.FindAsync(id);
            if (dbTag == null)
            {
                return NotFound();
            }

            return View(dbTag);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return View(tag);
            }

            if (id != tag.Id)
            {
                return BadRequest();
            }

            var dbTag = await _appDbContext.Tags.FindAsync(id);
            if (dbTag == null)
            {
                return NotFound();
            }

            bool isExist = await _appDbContext.Tags.AnyAsync(t => t.Name.ToLower().Trim() == tag.Name.ToLower().Trim() && t.Id != tag.Id);

            if (isExist)
            {
                ModelState.AddModelError("Name", "bu Adda Tag movcuddur");
                return View(dbTag);
            }

            dbTag.Name = tag.Name;
            dbTag.UpdatedAt = DateTime.Now;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbTag = await _appDbContext.Tags.FindAsync(id);
            if (dbTag == null)
            {
                return NotFound();
            }
            _appDbContext.Tags.Remove(dbTag);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
