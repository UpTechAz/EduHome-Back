using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class StaticFile
    {
        public int Id { get; set; }
        public string? HeaderLogo { get; set; }
        [NotMapped]
        public IFormFile HeaderLogoFile { get; set; }
    }
}
