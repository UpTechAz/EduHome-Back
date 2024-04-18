namespace WebApplication2.Models
{
    public class TeacherLink:BaseEntity
    {
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
        public int LinkId { get; set; }
        public Link? Link { get; set; }
        public string Url { get; set; }
        public int ContactInformationId { get; set; }
        public ContactInformation? ContactInformation { get; set; }
    }
}
