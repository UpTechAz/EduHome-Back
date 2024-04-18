namespace WebApplication2.Models
{
    public class Link : BaseEntity
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public IEnumerable<TeacherLink>? TeacherLinks { get; set; }
    }
}
