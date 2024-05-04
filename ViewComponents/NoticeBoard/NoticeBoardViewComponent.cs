using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;

namespace WebApplication2.ViewComponents.NoticeBoard
{
    public class NoticeBoardViewComponent: ViewComponent
    {
        private readonly AppDbContext _dbContext;
        public NoticeBoardViewComponent(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var noticeBoard = await _dbContext.NoticesBoards.ToListAsync();
            return View(noticeBoard);
        }
    }
}
