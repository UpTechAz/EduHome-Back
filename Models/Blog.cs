using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Blog:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string? FilePath { get; set; }
        [NotMapped]
        public IFormFile? Photo { get; set; }

        public DateTime Date { get; set; }
        //TODO:Bura duzelecek
        public int CommentCount { get; set; }


        
        public List<BlogComment>? BlogComment { get; set; }


    }
}
