using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Helpers;
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
            var skill = await _dbContext.Skills.ToListAsync();
            List<Skill> skills =await _dbContext.Skills.ToListAsync();
            ViewBag.Skillcount = skills.Count;
            return View(skill);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            Skill? skill = await _dbContext.Skills.FirstOrDefaultAsync();
            return View(skill);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Skill skill)
        {
            if (!ModelState.IsValid) return NotFound();
            await _dbContext.Skills.AddAsync(skill);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Skill? skill = await _dbContext.Skills.FirstOrDefaultAsync(x => x.Id == id);
            if (skill == null) return NotFound();
            var model = new Skill
            {
                Id = id,
                Language = skill.Language,
                TeamLeader = skill.TeamLeader,
                Development = skill.Development,
                Design = skill.Design,
                Innovation = skill.Innovation,
                Communication = skill.Communication,
                TeachersId = skill.TeachersId,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Skill skill,int id)
        {
            if(skill.Id != id) return BadRequest();
            if (!ModelState.IsValid) return View(skill);
            Skill? dbskill = await _dbContext.Skills.FirstOrDefaultAsync(s=>s.Id == id);
            //if (dbskill == null) return NotFound();
            dbskill.Language = skill.Language;
            dbskill.TeamLeader = skill.TeamLeader;
            dbskill.Development = skill.Development;
            dbskill.Design = skill.Design;
            dbskill.Innovation = skill.Innovation; 
            dbskill.Communication = skill.Communication;
            dbskill.TeachersId = skill.TeachersId;
            //await _dbContext.Skills.AddAsync(skill);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
