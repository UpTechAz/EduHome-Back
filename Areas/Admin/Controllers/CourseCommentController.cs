using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Superadmin")]
    public class CourseCommentController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CourseCommentController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var courseComments = await _appDbContext.CourseComments.ToListAsync();
            return View(courseComments);
        }
        [HttpPost]
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null) return BadRequest();
            var courseComment = await _appDbContext.CourseComments.FirstOrDefaultAsync(a => a.Id == id);
            if (courseComment == null) return NotFound();

            courseComment.IsApproved = courseComment.IsApproved ? false : true;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var courseComment = await _appDbContext.CourseComments.FindAsync(id);
            _appDbContext.CourseComments.Remove(courseComment);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
