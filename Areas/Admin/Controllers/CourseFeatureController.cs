using Microsoft.AspNetCore.Mvc;
using WebApplication2.DAL;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CourseFeatureController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CourseFeatureController(AppDbContext appDbContext )
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
