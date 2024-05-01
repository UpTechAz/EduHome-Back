using WebApplication2.Models;

namespace WebApplication2.ViewModels.Teachers
{
    public class TeacherVM
    {
        public List<Teacher> teachers {get;set;}
        public string FullName { get; set; }
        public string? FilePath { get; set; }
        public IFormFile? Photo { get; set; }
        public string ScientificDegree { get; set; }
    }
}
