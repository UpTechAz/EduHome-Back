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
        public async Task<IActionResult> Details(Teacher teacher)
        {
            teacher = await _dbContext.Teachers
                .Include(x=>x.TeacherLinks)
                .Include(x=>x.Skills)
                .Include(x=>x.ContactInformation).FirstOrDefaultAsync();
            teacher = new Teacher
            {
                FullName = teacher!.FullName,
                ScientificDegree = teacher.ScientificDegree,
                FilePath = teacher.FilePath,
                Photo = teacher.Photo,
                TeacherAbout = teacher.TeacherAbout,
                TeacherLinks = teacher.TeacherLinks,
                ContactInformation = teacher.ContactInformation,
                //TeacherLinks = teacher.TeacherLinks,
                Skills = teacher.Skills,
                Faculty = teacher.Faculty,
                Experience = teacher.Experience,
                Hobbies = teacher.Hobbies,
            };
            return View(teacher);
        }
    }
}
