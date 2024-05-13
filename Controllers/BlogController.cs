using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using WebApplication2.ViewModels.Blogs;
using WebApplication2.ViewModels.Course;

namespace WebApplication2.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _dbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public async Task<IActionResult> Index(int pageIndex=1)
        {
            IQueryable<Blog> queries = _dbContext.Blogs
;
            return View(PageNatedList<Blog>.Create(queries, pageIndex, 6, 6));

        }
        public async Task<IActionResult> LoadMore(int skipRow)
        {
            var blog = await _dbContext.Blogs
             .OrderByDescending(b => b.Id)
             .Skip(3 * skipRow)
             .Take(3)
             .ToListAsync();
            return PartialView("_BlogPartialView", blog);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            BlogIndexVM model = new()
            {
                Blogs = await _dbContext.Blogs
                .Include(x => x.BlogComment.Where(x => x.IsApproved))
                .FirstOrDefaultAsync(c => c.Id == id)
            };
;
            if (model is null) return NotFound();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Details(int id,BlogIndexVM? blogVM)
        {
            if(blogVM == null ) return NotFound();
            blogVM = new();
            if (id == null) { return NotFound(); }

            blogVM = new()
            {
                Blogs = await _dbContext.Blogs
                .Include(x => x.BlogComment)
                .FirstOrDefaultAsync(c => c.Id == id),
                BlogComments = blogVM.BlogComments,
                
            };
            blogVM.BlogComments.BlogId = id;
            if (!ModelState.IsValid) return View(blogVM);
            await _dbContext.BlogComments.AddAsync(blogVM.BlogComments);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
