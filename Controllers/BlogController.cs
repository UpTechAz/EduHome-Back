using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.ViewModels;
using WebApplication2.ViewModels.Blogs;

namespace WebApplication2.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index(int pageIndex = 1)
        {

            IQueryable<Blog> queries = _appDbContext.Blogs
;
            return View(PageNatedList<Blog>.Create(queries, pageIndex,6, 6));


        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) { return NotFound(); }



            BlogIndexVM blogVM = new()
            {
                Blogs = await _appDbContext.Blogs.FirstOrDefaultAsync(b => b.Id == id),
                BlogComments = await _appDbContext.BlogComments.Where(x=>x.BlogId == id).ToListAsync(),
                Categories = await _appDbContext.Categories.ToListAsync(),
                Tags = await _appDbContext.Tags.ToListAsync()
            };

            return View(blogVM);
        }
        [HttpGet]
        public async Task<IActionResult> Create ()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BlogComment blogComment)
        {
            if (!ModelState.IsValid) return View(blogComment);
            blogComment.CreatedAt = DateTime.Now;
            await _appDbContext.BlogComments.AddAsync(blogComment);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
