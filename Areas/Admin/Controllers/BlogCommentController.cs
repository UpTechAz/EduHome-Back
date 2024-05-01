using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class BlogCommentController : Controller
    {
        private readonly AppDbContext _context;

        public BlogCommentController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var blogComments = await _context.BlogComments
                //.Where(ec => ec.IsApproved)
                .ToListAsync();
            return View(blogComments);
        }
        [HttpPost]
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null) return BadRequest();
            var Blog = await _context.BlogComments.FirstOrDefaultAsync(a => a.Id == id);
            if (Blog == null) return NotFound();

            Blog.IsApproved = Blog.IsApproved ? false : true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            var Blog = await _context.BlogComments.FindAsync(id);
            if (Blog == null) return NotFound();

            _context.BlogComments.Remove(Blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }

}
