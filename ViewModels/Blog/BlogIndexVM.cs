using WebApplication2.Models;

namespace WebApplication2.ViewModels.Blogs
{
    public class BlogIndexVM
    {
        public Blog? Blogs { get; set; }
        public BlogComment BlogComments { get; set; }
        public List<Category>? Categories{ get; set; }
    }

}