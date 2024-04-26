using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class BlogCommentController : Controller
    {
        private readonly AppDbContext _context;

        public BlogCommentController(AppDbContext context)
        {
            _context = context;
        }

 
        public async Task<IActionResult> Index()
        {
            var blogComments = await _context.BlogComments.ToListAsync();
            return View(blogComments);
        }
        [HttpPost]
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null) return BadRequest();
            var blogComment = await _context.BlogComments.FirstOrDefaultAsync(a => a.Id == id);
            if (blogComment == null) return NotFound();

            blogComment.IsApproved = true;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        [HttpPost, ActionName("Delete")]
    
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogComment = await _context.BlogComments.FindAsync(id);
            _context.BlogComments.Remove(blogComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
