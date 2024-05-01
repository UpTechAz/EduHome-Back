namespace WebApplication2.Models
{
    public class CourseFeature : BaseEntity
    {
        public DateTime? Starts { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get;set; }
        public string Language { get; set; }
        public int StudentsCount { get; set; }
        public string Assesments { get; set; } 
        public int CourseFee { get; set; }
        public int CoursesId { get; set; }
        public Course? Courses { get; set; }
    }
}
