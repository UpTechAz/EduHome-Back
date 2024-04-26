using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NoticeBoardController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public NoticeBoardController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _appDbContext.NoticesBoards.ToListAsync();

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(NoticeBoard noticeBoard)
        {
            if (!ModelState.IsValid)
            {
                return View(noticeBoard);
            }

            noticeBoard.CreatedAt = DateTime.Now;
            await _appDbContext.NoticesBoards.AddAsync(noticeBoard);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var dbModel = await _appDbContext.NoticesBoards.FindAsync(id);
            if (dbModel == null)
            {
                return NotFound();
            }

            return View(dbModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, NoticeBoard noticeBoard)
        {
            if (!ModelState.IsValid)
            {
                return View(noticeBoard);
            }

            if (id != noticeBoard.Id)
            {
                return BadRequest();
            }

            var dbModel = await _appDbContext.NoticesBoards.FindAsync(id);
            if (dbModel == null)
            {
                return NotFound();
            }

            dbModel.DateTime = noticeBoard.DateTime;
            dbModel.Description = noticeBoard.Description;

            dbModel.ModifiedAt = DateTime.Now;
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbModel = await _appDbContext.NoticesBoards.FindAsync(id);
            if (dbModel == null)
            {
                return NotFound();
            }
            _appDbContext.NoticesBoards.Remove(dbModel);
            await _appDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
