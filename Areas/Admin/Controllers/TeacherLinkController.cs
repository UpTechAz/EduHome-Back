using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Constants;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class TeacherLinkController : Controller
    {
        private readonly AppDbContext _dbContext;
        public TeacherLinkController(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            List<TeacherLink>? teacherLink = await _dbContext.TeachersLink
                .Include(x => x.Teacher)
                .Include(x => x.Link) // include elemesek elaqeli modele gedib cixmir
                .ToListAsync();
            ViewBag.TeacherLink = teacherLink.Count;
            return View(teacherLink);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Links = await _dbContext.Links.ToListAsync();
            ViewBag.Teacher = await _dbContext.Teachers.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(TeacherLink teacherLink)
        {
            if (!ModelState.IsValid) return View(teacherLink);
            teacherLink.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _dbContext.TeachersLink.AddAsync(teacherLink);
            await _dbContext.SaveChangesAsync();
            ViewBag.Links = await _dbContext.Links.ToListAsync();
            ViewBag.Teacher = await _dbContext.Teachers.ToListAsync();
            return RedirectToAction(nameof(Index));

            //return View(teacherLink);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            TeacherLink? existTeacherLink = await _dbContext.TeachersLink
                .Include(x => x.Teacher) // include findAsyncle ishlemir
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existTeacherLink is null) return NotFound();
            ViewBag.Links = await _dbContext.Links.ToListAsync();
            return View(existTeacherLink);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, TeacherLink teacherLink)
        {
            if (id != teacherLink.Id) return BadRequest();

            if (!ModelState.IsValid) return View(teacherLink);

            TeacherLink? dbteacherLink = await _dbContext.TeachersLink
                .Include(x => x.Teacher)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (dbteacherLink is null) return NotFound();
            //dbteacherLink.TeacherId = teacherLink.TeacherId; // id vermemeliyik
            dbteacherLink.LinkId = teacherLink.LinkId;
            dbteacherLink.Url = teacherLink.Url.Trim();
            ViewBag.Links = await _dbContext.TeachersLink.ToListAsync();
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var dblink = await _dbContext.TeachersLink.FindAsync(id);
            if (dblink == null) return BadRequest();
            _dbContext.TeachersLink.Remove(dblink);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
