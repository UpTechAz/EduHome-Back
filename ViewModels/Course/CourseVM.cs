using WebApplication2.Models;

namespace WebApplication2.ViewModels.Course
{
    public class CourseVM
    {
        public Models.Course? Course { get; set; }
        public CourseComment? Comment { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Tag>? Tags { get; set; } 
        
    }
}
