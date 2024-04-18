using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Message : BaseEntity
    {

        [Required]
        public string Name { get; set; }
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Subject { get; set; }

        [Required]
        public string MessageInfo { get; set; }
    }
}
