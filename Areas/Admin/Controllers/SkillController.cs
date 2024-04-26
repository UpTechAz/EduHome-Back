using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Helpers;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class SkillController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SkillController(AppDbContext dbContext, IFileService fileService,
            IWebHostEnvironment _webHostEnvironment)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            List<Skill>? skill = await _dbContext.Skills
                .Include(x => x.Teacher)
                .ToListAsync();
            ViewBag.Skillcount = skill.Count;
            return View(skill);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Teacher = await _dbContext.Teachers.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Skill skill)
        {
            if (!ModelState.IsValid) return NotFound();

            ViewBag.Teacher = await _dbContext.Teachers.ToListAsync();

            await _dbContext.Skills.AddAsync(skill);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if(id == null) return BadRequest();

            Skill? skill = await _dbContext.Skills
                .Include(x => x.Teacher)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (skill == null) return NotFound();
            
            var model = new Skill
            {
                Language = skill.Language,
                TeamLeader = skill.TeamLeader,
                Development = skill.Development,
                Design = skill.Design,
                Innovation = skill.Innovation,
                Communication = skill.Communication,
                Teacher = await _dbContext.Teachers.FirstOrDefaultAsync(a => skill.TeacherId == a.Id),
                //TeachersId = skill.TeachersId,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Skill skill, int id)
        {
            if (skill.Id != id) return BadRequest();
            if (!ModelState.IsValid) return View(skill);
            Skill? dbskill = await _dbContext.Skills
                .Include(x => x.Teacher)
                .FirstOrDefaultAsync(s => s.Id == id);
            //if (dbskill == null) return NotFound();
            dbskill.Language = skill.Language;
            dbskill.TeamLeader = skill.TeamLeader;
            dbskill.Development = skill.Development;
            dbskill.Design = skill.Design;
            dbskill.Innovation = skill.Innovation;
            dbskill.Communication = skill.Communication;
            //await _dbContext.Skills.AddAsync(skill);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
