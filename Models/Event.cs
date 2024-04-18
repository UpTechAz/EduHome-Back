using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Event : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Venue { get; set; }
        public string? FilePath { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public int SpeakerId { get; set; }
        public Speaker? Speaker { get; set; }
    }
}
