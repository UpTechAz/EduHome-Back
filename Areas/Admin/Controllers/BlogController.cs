using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;
using WebApplication2.DAL;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;

        public BlogController(AppDbContext context, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _context.Blogs
                .Include(x=>x.BlogComment)
                .ToListAsync();

            return View(blogs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (!ModelState.IsValid) return View(blog);

            if (blog.Photo != null)
            {
                if (!_fileService.IsImage(blog.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(blog);
                }

                int maxSize = 180;
                if (!_fileService.CheckSize(blog.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(blog);
                }

                try
                {
                    var filename = await _fileService.UploadAsync(blog.Photo);
                    blog.FilePath = filename;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while uploading the file.");

                    return View(blog);
                }
            }

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return NotFound();
            return View(blog);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Blog blog)
        {
            if (!ModelState.IsValid) return View(blog);


            var existingBlog = await _context.Blogs.FindAsync(blog.Id);
            if (existingBlog == null) return NotFound();


            existingBlog.Title = blog.Title;
            existingBlog.Description = blog.Description;
            existingBlog.Date = blog.Date;
            existingBlog.Author=blog.Author;
            existingBlog.CommentCount= blog.CommentCount;


            if (blog.Photo != null)
            {

                if (!string.IsNullOrEmpty(existingBlog.FilePath))
                {
                    _fileService.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", existingBlog.FilePath));
                }


                int maxSize = 180;
                if (!_fileService.IsImage(blog.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(blog);
                }
                if (!_fileService.CheckSize(blog.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(blog);
                }

                try
                {
                    var filename = await _fileService.UploadAsync(blog.Photo);
                    existingBlog.FilePath = filename;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while uploading the file.");

                    return View(blog);
                }
            }


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null) return NotFound();

            if (!string.IsNullOrEmpty(blog.FilePath))
            {
                _fileService.Delete(Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", blog.FilePath));
            }

            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}