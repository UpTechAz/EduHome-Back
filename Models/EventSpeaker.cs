using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class EventSpeaker : BaseEntity
    {
        [Required(ErrorMessage = "Speaker is required")]
        public int SpeakerId { get; set; }
        public Speaker? Speaker { get; set; }

        [Required(ErrorMessage = "Event is required")]
        public int EventId { get; set; }
        public Event? Event { get; set; }

    }
}
