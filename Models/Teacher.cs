using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Teacher : BaseEntity
    {
        public string FullName { get; set; }
        public string? FilePath { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string TeacherAbout { get; set; }    
        public string Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        public string ScientificDegree { get; set; }
        public IEnumerable<TeacherLink>? TeacherLinks { get; set; }
        public ContactInformation? ContactInformation { get; set; }
    }
}
