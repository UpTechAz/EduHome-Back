using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Course : BaseEntity
    {
        public string CoursName { get; set; }
        public string CoursApply { get; set; }
        public string Certification { get; set; }
        public string CoursAbout { get; set; }
        public string? FilePath { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public int CourseFeaturedId { get; set; }
        public CourseFeature? CoursFeature { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<CourseComment>? CourseComment { get; set; }

    }
}
