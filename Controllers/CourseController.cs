using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.ViewModels;

namespace WebApplication2.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CourseController(AppDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        public async Task<IActionResult> Index(int? id,int pageIndex = 1)
        {
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


            //if (id is not null)
            //{
            //    var cateCourse = await _dbContext.Courses
            //                            .Where(x => x.CategoryId == id)
            //                            .OrderByDescending(x => x.Id)
            //                            .Take(3)
            //                            .ToListAsync();
            //    return View(cateCourse);
            //}
            //var course = await _dbContext.Courses
            //    .OrderByDescending(c => c.Id)
            //    .Take(3)
            //    .ToListAsync();
            //return View(course);
        }
        //public async Task<IActionResult> LoadMore(int skipRow, int? id)
        //{
        //    if (id is not null)
        //    {
        //        var cateCourse = await _dbContext.Courses
        //                                .Where(x => x.CategoryId == id)
        //                                .OrderByDescending(x => x.Id)
        //                                .Skip(3 * skipRow)
        //                                .Take(3)
        //                                .ToListAsync();
        //        return PartialView("_CoursePartialView", cateCourse);
        //    }

        //    var course = await _dbContext.Courses
        //        .OrderByDescending(c => c.Id)
        //        .Skip(3 * skipRow)
        //        .Take(3)
        //        .ToListAsync();
        //    return PartialView("_CoursePartialView", course);
        //}
        public async Task<IActionResult> Details(Course? Course, int? id)
        {
            if (id == null) return NotFound();
            Course = await _dbContext.Courses
                .Include(x => x.CoursFeature)
                .Include(x => x.Category)
                .Include(x => x.CourseComment)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (Course == null) return NotFound();
            var model = new Course
            {
                FilePath = Course.FilePath,
                CoursName = Course.CoursName,
                CoursFeature = Course.CoursFeature,
                CoursAbout = Course.CoursAbout,
                CoursApply = Course.CoursApply,
                Certification = Course.Certification,
                Category = Course.Category,
                CourseComment = Course.CourseComment,

            };
            return View(model);
        }



    }
}
