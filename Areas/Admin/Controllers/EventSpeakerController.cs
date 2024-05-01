using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class EventSpeakerController : Controller
    {
        private readonly AppDbContext _context;

        public EventSpeakerController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var eventSpeakers = await _context.EventSpeakers
                .Include(es => es.Speaker)
                .Include(es => es.Event)
                .ToListAsync();

            return View(eventSpeakers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Speakers = _context.Speakers.ToList();
            ViewBag.Events = _context.Events.ToList();
            return View();
        }

        [HttpPost]
 
        public async Task<IActionResult> Create([Bind("SpeakerId, EventId")] EventSpeaker eventSpeaker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventSpeaker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Speakers = _context.Speakers.ToList();
            ViewBag.Events = _context.Events.ToList();
            return View(eventSpeaker);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventSpeaker = await _context.EventSpeakers.FindAsync(id);
            if (eventSpeaker == null)
            {
                return NotFound();
            }

            ViewBag.Speakers = _context.Speakers.ToList();
            ViewBag.Events = _context.Events.ToList();
            return View(eventSpeaker);
        }
        [HttpPost]
  
        public async Task<IActionResult> Update(int id, EventSpeaker eventSpeaker)
        {
            if (id != eventSpeaker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingEventSpeaker = await _context.EventSpeakers.FindAsync(id);
                    if (existingEventSpeaker == null)
                    {
                        return NotFound();
                    }

                    existingEventSpeaker.SpeakerId = eventSpeaker.SpeakerId;
                    existingEventSpeaker.EventId = eventSpeaker.EventId;

                    _context.Update(existingEventSpeaker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventSpeakerExists(eventSpeaker.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Speakers = _context.Speakers.ToList();
            ViewBag.Events = _context.Events.ToList();
            return View(eventSpeaker);
        }




        [HttpPost]
     
        public async Task<IActionResult> Delete(int id)
        {
            var eventSpeaker = await _context.EventSpeakers.FindAsync(id);
            if (eventSpeaker == null)
            {
                return NotFound();
            }
            _context.EventSpeakers.Remove(eventSpeaker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventSpeakerExists(int id)
        {
            return _context.EventSpeakers.Any(e => e.Id == id);
        }
    }
}
