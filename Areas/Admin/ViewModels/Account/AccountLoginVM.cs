using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Areas.Admin.ViewModels.Account
{
    public class AccountLoginVM
    {

        [Required, Display(Name = "User Name")]
        public string Username { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
