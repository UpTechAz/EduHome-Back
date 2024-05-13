using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Speaker : BaseEntity
    {
        [Required(ErrorMessage = "FullName is required")]
        public string? FullName { get; set; }
        public string? FilePath { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }
        [Required(ErrorMessage = "WorkPlace is required")]
        public string? WorkPlace { get; set; }
        [Required(ErrorMessage = "Profession is required")]
        public string? Profession { get; set; }

     
        public ICollection<EventSpeaker>? EventSpeakers { get; set; }
    }
}
