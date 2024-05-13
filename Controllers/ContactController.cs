using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.ViewModels.Contact;

namespace WebApplication2.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _dbcontext;
        public ContactController(AppDbContext appDbContext)
        {
            _dbcontext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            ContactIndexVM contactVM = new ContactIndexVM
            {
                Messages = await _dbcontext.Messages.FirstOrDefaultAsync(),
            };
            return View(contactVM);
        }
        [HttpPost]
        public async Task<IActionResult> Index(ContactIndexVM model)
        {
            if (!ModelState.IsValid) return View();

            model = new ()
            {
               Messages = model.Messages,
            };
            
            await _dbcontext.Messages.AddAsync(model.Messages);
            await _dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
