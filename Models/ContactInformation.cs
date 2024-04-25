using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class ContactInformation : BaseEntity
    {
        [DataType(DataType.EmailAddress)]
        public  string Email { get; set; }
        public string Number { get; set; }
        public string Skype { get; set; }
        public Teacher? Teacher { get; set; }
        public int TeacherId { get; set; }

    }
}
