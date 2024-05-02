using WebApplication2.Models;

namespace WebApplication2.ViewModels.Teachers
{
    public class TeacherVM
    {
        public List<Teacher> Teachers {get;set;}
        public List<TeacherLink> TeacherLinks { get;set;}
        public List<Skill> Skills { get;set;}
    }
}
