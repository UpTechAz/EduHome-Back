using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace WebApplication2.Areas.Admin.ViewModels
{
    public class AccountLoginVM
    {
        [Required, Display(Name = "User Name")]
        public string Username { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
