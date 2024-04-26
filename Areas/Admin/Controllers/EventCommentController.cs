using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EventCommentController : Controller
    {
        private readonly AppDbContext _context;

        public EventCommentController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var eventComments = await _context.EventComments
                .Where(ec => ec.IsApproved)
                .ToListAsync();
            return View(eventComments);
        }
        [HttpPost]
             public async Task<IActionResult> Approve(int? id)
        {
            if (id == null) return BadRequest();
            var eventComment = await _context.EventComments.FirstOrDefaultAsync(a => a.Id == id);
            if (eventComment == null) return NotFound();

            eventComment.IsApproved = true; 
            await _context.SaveChangesAsync(); 

            return RedirectToAction(nameof(Index));
        }



        [HttpPost]

        public async Task<IActionResult> Delete(int id)
        {
            var eventComment = await _context.EventComments.FindAsync(id);
            if (eventComment == null) return NotFound();

            _context.EventComments.Remove(eventComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }


}
