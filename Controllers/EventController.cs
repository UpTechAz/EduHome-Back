using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.ViewModels.Event;

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
            List<Event> @event = await _dbContext.Events
                .OrderByDescending(e => e.Id)
                .Take(3)
                .ToListAsync();
            return View(@event);
        }
        public async Task<IActionResult> LoadMore(int skipRow)
        {
            var @event = await _dbContext.Events
                .OrderByDescending(e => e.Id)
                .Skip(3 * skipRow)
                .Take(3)
                .ToListAsync();
            return PartialView("_EventPartialView", @event);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            EventVM @event = new()
            {
                Events = await _dbContext.Events
                .Include(x => x.EventComment)
                .Include(x => x.EventSpeakers.Where(x => x.EventId == id)).ThenInclude(x => x.Speaker)
               .FirstOrDefaultAsync(c => c.Id == id)
            };           

            if (@event == null) return NotFound();
            return View(@event);
        }
        [HttpPost]
        public async Task<IActionResult> Details(EventVM @event, int id)
        {
            if (@event == null) { return NotFound(); }
            @event = new()
            {
                Events = await _dbContext.Events
                .Include(x => x.EventComment)
                .Include(x => x.EventSpeakers.Where(x => x.EventId == id)).ThenInclude(x => x.Speaker)
               .FirstOrDefaultAsync(c => c.Id == id),
                EventComments = @event.EventComments,
            };
            @event.EventComments.EventId = id;
            if (!ModelState.IsValid) return View(@event);
            await _dbContext.EventComments.AddAsync(@event.EventComments);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}