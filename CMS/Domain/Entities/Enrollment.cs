
namespace CMS.Domain.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public Student Student { get; set; } = null!;
        public int StudentId { get; set; } // Foreign key to Student
        public Course Course { get; set; } = null!;
        public int CourseId { get; set; } // Foreign key to Course
        public DateTime EnrolledAt { get; set; }
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;

    }
}
