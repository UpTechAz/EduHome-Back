using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class MessageController : Controller
    {
        private readonly AppDbContext _context;
        public MessageController(AppDbContext context)
        {
            _context = context;
        }   
        public async Task<IActionResult> Index()
        {
            var messages = await _context.Messages.ToListAsync();
            return View(messages);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound(); 
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return View(nameof(Index));
        }
    }
}
