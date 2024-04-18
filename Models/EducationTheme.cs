using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class EducationTheme : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? FilePath { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
    }
}
