using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;

namespace WebApplication2.ViewComponents.StudentQuote
{
    public class StudentQuoteViewComponent : ViewComponent
    {
        private readonly AppDbContext _appDbContext;
        public StudentQuoteViewComponent(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var studentQuotes = await _appDbContext.StudentQuote.ToListAsync();
            return View(studentQuotes);
        }
    }
}
