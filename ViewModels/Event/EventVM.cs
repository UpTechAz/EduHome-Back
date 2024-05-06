using WebApplication2.Models;

namespace WebApplication2.ViewModels.Event
{
    public class EventVM
    {
        public List<Models.Event> Events { get; set;}
        public List<EventComment> EventComments { get; set;}
        public List<EventSpeaker> EventSpeaker { get; set;}
    }
}
