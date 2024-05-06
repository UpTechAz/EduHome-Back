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
            return View(PageNatedList<Blog>.Create(queries, pageIndex,5, 5));


        }

        public async Task<IActionResult> BlogDetails(int? id)
        {
            if (id == null) { return NotFound(); }

            BlogIndexVM blogVM = new()
            {
                Blogs = await _appDbContext.Blogs.FirstOrDefaultAsync(b => b.Id == id),
                BlogComments = await _appDbContext.BlogComments.Where(x=>x.BlogId == id).ToListAsync(),
                Categories = await _appDbContext.Categories.ToListAsync()
            };

            return View(blogVM);
        }


    }
}
