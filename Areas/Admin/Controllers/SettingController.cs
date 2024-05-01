using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Helpers;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAmdin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _dbContext;

        public SettingController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _dbContext.Settings.ToListAsync();
            ViewBag.SettingInfo = model.Count;
            return View(model);
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Setting setting)
        {
            if(!ModelState.IsValid) return NotFound();
            await _dbContext.Settings.AddAsync(setting);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Update 
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            Setting? setting = await _dbContext.Settings.FirstOrDefaultAsync(x => x.id == id);
            if (setting == null) return BadRequest();
            var model = new Setting
            {
                Value = setting.Value,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, Setting setting)
        {
            if (setting.id != id) return BadRequest();
            if (!ModelState.IsValid) return View(setting);
            Setting? dbSetting = await _dbContext.Settings.FirstOrDefaultAsync(x =>x.id == id);
            dbSetting.Value = setting.Value;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            var dbSetting = await _dbContext.Settings.FindAsync(id);
            if (dbSetting == null) return NotFound();
            _dbContext.Settings.Remove(dbSetting);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
