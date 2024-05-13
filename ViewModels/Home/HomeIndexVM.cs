using WebApplication2.Models;

namespace WebApplication2.ViewModels.Home
{
    public class HomeIndexVM
    {
        public List<Slider>? Sliders { get; set; }
        public List<Blog>? Blogs { get; set; }
        public List<Models.Event>? Events { get; set; }
        public List<NoticeBoard>? NoticeBoards { get; set; }
        public List<Models.Course>? Courses { get; set; }
        public EducationTheme? EducationTheme { get; set; }
        public StudentQuote? StudentQuote { get; set; }
    }
}
