namespace CMS.Application.Interfaces
{
    public interface ICourseService
    {
        public Task<Result<IEnumerable<CourseReadDto>>> GetCoursesAsync();
        public Task<Result<CourseReadDto>> GetCourseByIdAsync(int id);
        public Task<Result<CourseReadDto>> CreateCourseAsync(CourseCreateDto courseDto);
        public Task<Result<CourseReadDto>> UpdateCourseAsync(int id, CourseUpdateDto courseDto);
        public Task<Result> DeleteCourseAsync(int id);



    }
}
