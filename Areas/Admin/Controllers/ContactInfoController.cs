using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Helpers;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class ContactInfoController : Controller
    {
        private readonly AppDbContext _dbContext;
        //private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        public ContactInfoController(AppDbContext appDbContext,
            IFileService fileService)
        {
            _dbContext = appDbContext;
            //_webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }
        public async Task<IActionResult> Index()
        {
            List<ContactInformation> contactInfos = await _dbContext.ContactInformation.Include(x=>x.Teacher).ToListAsync();
            ViewBag.ContactInfo = contactInfos.Count;
            return View(contactInfos);
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ContactInformation contactInfo)
        {
            if (!ModelState.IsValid) return View(contactInfo);
            await _dbContext.ContactInformation.AddAsync(contactInfo);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var ContactInfo = await _dbContext.ContactInformation.
                FirstOrDefaultAsync(c => c.Id == id);
            if (ContactInfo == null) return NotFound();
            
            return View(ContactInfo);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ContactInformation contactInfo, int id)
        {
            if(id != contactInfo.Id) return BadRequest();
            if(!ModelState.IsValid) return View(contactInfo);
            var dbContactInfo = await _dbContext.ContactInformation.FindAsync(id);
            dbContactInfo.Email = contactInfo.Email;
            dbContactInfo.Number = contactInfo.Number;
            dbContactInfo.Skype = contactInfo.Skype;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            var dbContactinfo = await _dbContext.ContactInformation.FindAsync(id);
            if(dbContactinfo ==  null) return BadRequest();
            _dbContext.ContactInformation.Remove(dbContactinfo);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion 
    }
}
