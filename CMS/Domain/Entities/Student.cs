using System.ComponentModel.DataAnnotations;

namespace CMS.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
