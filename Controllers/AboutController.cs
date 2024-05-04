using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.ViewModels.About;

namespace WebApplication2.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _dbContext;
        public AboutController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            AboutIndexVM aboutIndexVM = new AboutIndexVM
            {
                EducationTheme = await _dbContext.EducationTheme.FirstOrDefaultAsync(),
                Teachers = await _dbContext.Teachers.Skip(1).Take(4).ToListAsync(),

            };
            return View(aboutIndexVM);
        }
    }
}
