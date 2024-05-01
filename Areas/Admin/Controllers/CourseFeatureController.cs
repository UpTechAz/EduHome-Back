using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]

    public class CourseFeatureController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public CourseFeatureController(AppDbContext appDbContext )
        {
            _appDbContext = appDbContext;
        }

        public async Task<IActionResult> Index()
        {
            List<CourseFeature> courseFeature = await _appDbContext.CourseFeature
                .Include(x => x.Courses).ToListAsync();
            ViewBag.Coursefeature = courseFeature.Count;
            return View(courseFeature);
        }
        #region Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CourseFeature courseFeature)
        {
            if(!ModelState.IsValid) return View(courseFeature);
            await _appDbContext.CourseFeature.AddAsync(courseFeature);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var courfeature = await _appDbContext.CourseFeature
                .FirstOrDefaultAsync(c=>c.Id == id);
            if (courfeature == null) return BadRequest();

            return View(courfeature);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CourseFeature courseFeature, int id)
        {
            if(id != courseFeature.Id) return BadRequest();
            if(!ModelState.IsValid) return View(courseFeature);
            var dbcoursFeature = await _appDbContext.CourseFeature.FindAsync(id);
            dbcoursFeature.CourseFee = courseFeature.CourseFee;
            dbcoursFeature.SkillLevel = courseFeature.SkillLevel;
            dbcoursFeature.Duration = courseFeature.Duration;
            dbcoursFeature.ClassDuration = courseFeature.ClassDuration;
            dbcoursFeature.Language = courseFeature.Language;
            dbcoursFeature.StudentsCount = courseFeature.StudentsCount;
            dbcoursFeature.Assesments = courseFeature.Assesments;
            dbcoursFeature.Starts = courseFeature.Starts;
            dbcoursFeature.CoursesId = courseFeature.CoursesId;
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
        #region Delete
        public async Task<IActionResult> Delete(int id)
        {
            var dbcoursFeature = await _appDbContext.CourseFeature.FindAsync(id);
            if(dbcoursFeature == null) return BadRequest();
            _appDbContext.CourseFeature.Remove(dbcoursFeature);
            await _appDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
