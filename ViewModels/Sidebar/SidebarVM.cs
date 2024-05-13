using WebApplication2.Models;

namespace WebApplication2.ViewModels.Sidebar
{
    public class SidebarVM
    {
        public List<Category>? Categories { get; set; }
        //public List<Models.Course>? Courses { get; set; }
        public List<Tag>? Tags { get; set; }
        public List<Blog>? Blogs { get; set; }
    }
}
