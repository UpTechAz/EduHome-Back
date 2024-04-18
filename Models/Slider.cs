using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Slider : BaseEntity
    {
        [NotMapped] 
        public IFormFile? Photo { get; set; }
        public string? BackgroundImage { get; set; }
        public string? FilePath { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
