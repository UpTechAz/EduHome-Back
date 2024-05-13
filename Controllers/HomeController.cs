using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.ViewModels.Home;

namespace WebApplication2.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _dbContext;
        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
		{
			HomeIndexVM homeIndexVM = new HomeIndexVM
			{
				Sliders = await _dbContext.Sliders.ToListAsync(),
				Events = await _dbContext.Events.Take(3).ToListAsync(),
				Blogs = await _dbContext.Blogs.Take(3).ToListAsync(),
				NoticeBoards = await _dbContext.NoticesBoards.ToListAsync(),
				Courses = await _dbContext.Courses.Take(3).ToListAsync(),
				EducationTheme = await _dbContext.EducationTheme.FirstOrDefaultAsync(),
				StudentQuote = await _dbContext.StudentQuote.FirstOrDefaultAsync(),
			};
			return View(homeIndexVM);
		}

	}
}
