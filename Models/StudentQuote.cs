using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class StudentQuote : BaseEntity
    {
        public string? FilePath { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
    }
}
