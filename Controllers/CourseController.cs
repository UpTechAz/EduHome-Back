using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Net;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.ViewModels;
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
        public async Task<IActionResult> Index(int? id, int pageIndex = 1 )
        {
            //ViewData["NameSortParm"] = !String.IsNullOrEmpty(sortOrder) ? sortOrder : "";
            //var course = await _dbContext.Courses
            //     .OrderByDescending(c => c.Id)
            //     .Take(3)
            //     .ToListAsync();
            //if (!String.IsNullOrWhiteSpace(sortOrder))
            //{
            //    course.Any(x => x.CoursName.Contains(sortOrder));
            //}

            ////var model = FilterByTitle(course.CourseName)
            //return View(course);
            if (id is not null)
            {
                IQueryable<Course> cateCourse = _dbContext.Courses
                                        .Where(x => x.CategoryId == id)
                                        .OrderByDescending(x => x.Id);
                return View(PageNatedList<Course>.Create(cateCourse, pageIndex, 6, 6));
            }
            IQueryable<Course> queries = _dbContext.Courses
;
            return View(PageNatedList<Course>.Create(queries, pageIndex, 6, 6));

        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            CourseVM model = new()
            {
                Course = await _dbContext.Courses
                .Include(x => x.CoursFeature)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(c => c.Id == id),
                Comments = await _dbContext.CourseComments.Where(x => x.CourseID == id && x.IsApproved).ToListAsync(),
            };

            if (model is null) return NotFound();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Details(CourseVM model, int id)
        {
            if (model is null) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            model = new()
            {
                Course = await _dbContext.Courses
                .Include(x => x.CoursFeature)
                .Include(x => x.Category)
                .Include(x => x.CourseComment.Where(x => x.IsApproved))
                .FirstOrDefaultAsync(c => c.Id == id),
                Comment = model.Comment
            };
            model.Comment.CourseID = id;
            model.Comment.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _dbContext.CourseComments.AddAsync(model.Comment);
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
