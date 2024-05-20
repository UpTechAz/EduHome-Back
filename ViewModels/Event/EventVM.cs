using WebApplication2.Models;

namespace WebApplication2.ViewModels.Event
{
    public class EventVM
    {
        public Models.Event? Events { get; set;}
        public EventComment? EventComments { get; set;}
        public List<EventSpeaker>? EventSpeaker { get; set;}
        public List<Category>? Categories { get; set; }
        public List<Tag>? Tags { get; set; }
    }
}
