using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;

namespace WebApplication2.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _dbContext;
        public EventController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var @event = await _dbContext.Events
                .Include
                .OrderByDescending(e => e.Id)
                .Take(3)
                .ToListAsync();
            return View(@event);
        }
    }
}