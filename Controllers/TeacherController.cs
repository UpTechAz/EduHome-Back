using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;
using WebApplication2.ViewModels.Teachers;

namespace WebApplication2.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _dbContext;
        public TeacherController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            TeacherVM teacherVM = new TeacherVM
            {
                Teachers = await _dbContext.Teachers.ToListAsync()
            };
            return View(teacherVM);
        }
        public async Task<IActionResult> Details(Teacher? teacher)
        {
            
            teacher = await _dbContext.Teachers
                 .Include(x => x.TeacherLinks)
                 .Include(x => x.Skills)
                 .Include(x => x.ContactInformation).FirstOrDefaultAsync();

            teacher = new Teacher
            {
                FullName = teacher!.FullName,
                ScientificDegree = teacher.ScientificDegree,
                FilePath = teacher.FilePath,
                Photo = teacher.Photo,
                TeacherAbout = teacher.TeacherAbout,
                TeacherLinks = teacher.TeacherLinks,
                ContactInformation = teacher.ContactInformation,
                Skills = teacher.Skills,
                Faculty = teacher.Faculty,
                Experience = teacher.Experience,
                Hobbies = teacher.Hobbies,
            };
            return View(teacher);
            //if (id == null) { return NotFound(); }
            //var Teacher = await _dbContext.Teachers
            //    .Include(x => x.ContactInformation)
            //    .Include(x => x.Skills)
            //    .Include(x => x.TeacherLinks)
            //   .FirstOrDefaultAsync(c => c.Id == id);
            //if (Teacher == null) return NotFound();
            //var model = new Teacher
            //{
            //    FullName = Teacher.FullName,
            //    FilePath = Teacher.FilePath,
            //    Experience = Teacher.Experience,
            //    TeacherAbout = Teacher.TeacherAbout,
            //    Hobbies = Teacher.Hobbies,
            //    Faculty = Teacher.Faculty,
            //    ScientificDegree = Teacher.ScientificDegree,
            //    ContactInformation = Teacher.ContactInformation,
            //};
            //return View(model);

        }
    }
}
