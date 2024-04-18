namespace WebApplication2.Models
{
    public class Category : BaseEntity
    {
        public string Title { get; set; }
        public List<Course>? Courses { get; set; }
    }
}
