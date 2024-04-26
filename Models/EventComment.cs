namespace WebApplication2.Models
{
    public class  EventComment: BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; } 
        public string Subject { get; set; }
        public string MessageInfo { get; set; }
        public bool IsApproved { get; set; } = false;
        public int EventId { get; set; }
        public Event? Event { get; set; }
    }
}
