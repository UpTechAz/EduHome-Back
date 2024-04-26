namespace WebApplication2.Models
{
    public class BlogComment:BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }

        public string MessageInfo { get; set; }
        public bool IsApproved { get; set; } = false;
    
        public int BlogId { get; set; }

        public Blog? Blog { get; set; }
    }
}
