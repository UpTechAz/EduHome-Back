using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Setting
    {
        public int id { get; set; }
        [StringLength(255)]
        public string Key { get; set; }
        [StringLength(1000)]
        public string Value { get; set; }


    }
}
