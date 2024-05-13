using System.ComponentModel.DataAnnotations;
using WebApplication2.Models;
namespace WebApplication2.ViewModels.Contact
{
    public class ContactIndexVM
    {
        public Message Messages { get; set; }
        //public int Id { get; set; }
        //public string Name { get; set; }
        //[DataType(DataType.EmailAddress)]
        //public string Email { get; set; }
        //public string MessageInfo { get; set; }
        //public string Subject { get; set; }
    }
}
