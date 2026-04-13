namespace CMS.Application.Interfaces
{
    public interface IEnrollmentService
    {
        Task<Result> EnrollAsync(int studentId, int courseId);
        Task<Result<IEnumerable<StudentReadDto>>> GetStudentsByCourseIdAsync(int courseId);
        Task<Result<IEnumerable<CourseReadDto>>> GetCoursesByStudentIdAsync(int studentId, EnrollmentStatus? status = null);
        Task<Result> CompleteEnrollmentAsync(int studentId, int courseId);
        Task<Result> DropEnrollmentAsync(int studentId, int courseId);
        Task<Result> CancelEnrollmentAsync(int studentId, int courseId);
    }
}
