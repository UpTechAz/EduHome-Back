using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.ViewModels.Teachers;

namespace WebApplication2.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _dbContext;
        public TeacherController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            TeacherVM teacherVM = new TeacherVM
            {
                Teachers = await _dbContext.Teachers
                .OrderByDescending(t => t.Id)
                .Take(4)
                .ToListAsync()
            };
            return View(teacherVM);
        }
        public async Task<IActionResult> LoadMore(int skipRow)
        {
            var Teacher = await _dbContext.Teachers
             .OrderByDescending(t => t.Id)
             .Skip(4 * skipRow)
             .Take(4)
             .ToListAsync();
            return PartialView("_TeacherPartialView", Teacher);
        }
        public async Task<IActionResult> Details(Teacher? Teacher,int? id)
        {
            if (id == null) { return NotFound(); }
            Teacher = await _dbContext.Teachers
                .Include(x => x.ContactInformation)
                .Include(x => x.Skills)
                .Include(x => x.TeacherLinks)
               .FirstOrDefaultAsync(c => c.Id == id);
            if (Teacher == null) return NotFound();
            return View(Teacher);
        }
    }
}
