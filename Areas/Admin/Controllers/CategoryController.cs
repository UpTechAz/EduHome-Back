using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult>Index()
        {
            var model = await _appDbContext.Categories.ToListAsync();

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            bool isExist = await _appDbContext.Categories.AnyAsync(c => c.Title == category.Title);

            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu adda Category movcuddur");
                return View(category);
            }
            category.CreatedAt = DateTime.Now;
            await _appDbContext.Categories.AddAsync(category);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var dbcategory = await _appDbContext.Categories.FindAsync(id);
            if (dbcategory == null)
            {
                return NotFound();
            }

            return View(dbcategory);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            if (id != category.Id)
            {
                return BadRequest();
            }

            var dbCategory = await _appDbContext.Categories.FindAsync(id);
            if (dbCategory == null)
            {
                return NotFound();
            }

            bool isExist = await _appDbContext.Categories.AnyAsync(rw => rw.Title.ToLower().Trim() == category.Title.ToLower().Trim() && rw.Id != category.Id);

            if (isExist)
            {
                ModelState.AddModelError("Title", "bu Adda Title movcuddur");
                return View(dbCategory);
            }

            dbCategory.Title = category.Title;

            dbCategory.UpdatedAt = DateTime.Now;
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbCategory = await _appDbContext.Categories.FindAsync(id);
            if (dbCategory == null)
            {
                return NotFound();
            }
            _appDbContext.Categories.Remove(dbCategory);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
