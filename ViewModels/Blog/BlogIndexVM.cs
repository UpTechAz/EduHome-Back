using WebApplication2.Models;

namespace WebApplication2.ViewModels.Blogs
{
    public class BlogIndexVM
    {
        public Blog? Blogs { get; set; }
        public List<BlogComment>? BlogComments { get; set; }
        public List<Category>? Categories{ get; set; }
        public List<Tag>? Tags{ get; set; }
    }

}