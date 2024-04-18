using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Speaker : BaseEntity
    {
        public string FullName { get; set; }    
        public string FilePath { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        public string WorkPlace { get; set; }
        public int EventId { get; set; }
        public Event? Events { get; set; }

    }
}
