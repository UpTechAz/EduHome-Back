using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Net;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.ViewModels.Course;

namespace WebApplication2.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CourseController(AppDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<IActionResult> Index(string sortOrder)
        {
            //ViewData["NameSortParm"] = !String.IsNullOrEmpty(sortOrder) ? sortOrder : "";
            var course = await _dbContext.Courses
                 .OrderByDescending(c => c.Id)
                 .Take(3)
                 .ToListAsync();
            if (!String.IsNullOrWhiteSpace(sortOrder))
            {
                course.Any(x => x.CoursName.Contains(sortOrder));
            }

            //var model = FilterByTitle(course.CourseName)
            return View(course);
            //if (String.IsNullOrEmpty(sortOrder))
            //{
            //    var dataContext = _dbContext.Courses
            //        .OrderByDescending(c => c.Id)
            //        .Take(3)
            //        .ToListAsync();
            //    return View(dataContext);
            //}
            //else
            //{
            //    var searchItems = await _dbContext.Courses
            //        .OrderByDescending(c => c.Id)
            //        .Take(3)
            //        .Where(c=>c.CoursName.Contains(sortOrder))
            //        .ToListAsync();
            //    return View(searchItems);
            //}
        }
        public async Task<IActionResult> LoadMore(int skipRow)
        {
            var course = await _dbContext.Courses
                .OrderByDescending(c => c.Id)
                .Skip(3 * skipRow)
                .Take(3)
                .ToListAsync();
            return PartialView("_CoursePartialView", course);
        }
        public async Task<IActionResult> Details( int? id)
        {
            if (id == null) return NotFound();
            CourseVM model = new()
            {
                Course = await _dbContext.Courses
                .Include(x => x.CoursFeature)
                .Include(x => x.Category)
                .Include(x => x.CourseComment.Where(x => x.IsApproved))
                .FirstOrDefaultAsync(c => c.Id == id)
            };

            if (model is null) return NotFound();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Details(CourseVM courseVM, int id)
        {
            if (courseVM.Comment is null) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View("Index", courseVM);
            }

            courseVM = new()
            {
                Comment = courseVM.Comment
            };

            CourseComment comment = courseVM.Comment;
            comment.CourseID = id;
            comment.CreatedAt = DateTime.UtcNow.AddHours(4);
            //var comment = new CourseComment
            //{
            //    Name = courseVM.Comment.Name,
            //    Email = courseVM.Comment.Email,
            //    Subject = courseVM.Comment.Subject,
            //    MessageInfo = courseVM.Comment.MessageInfo,
            //    CourseID = id,
            //    CreatedAt = DateTime.UtcNow.AddHours(4),
            //};
            await _dbContext.CourseComments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<PartialViewResult> FilterByTitle([FromBody] Payload payload)
        {
            var search = await _dbContext.Courses
                 .Where(p => !string.IsNullOrEmpty(payload.title) ? p.CoursName.Contains(payload.title) : true).ToListAsync();

            return PartialView("_CoursePartialView", search);
        }
    }
}
