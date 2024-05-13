using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class CourseComment : BaseEntity
    {
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Subject { get; set; }
        public string MessageInfo { get; set; }
        public bool IsApproved { get; set; } = false;  
        public int CourseID { get; set; }
        public Course? Course { get; set; }

    }
}
