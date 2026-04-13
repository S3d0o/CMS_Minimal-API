
namespace CMS.Application.Services
{
    public class CourseService(CMS_DBContext db, IMapper mapper, ILogger<CourseService> logger) : ICourseService
    {
        public async Task<Result<IEnumerable<CourseReadDto>>> GetCoursesAsync()
        {
            var courses = await db.Courses
                .AsNoTracking()
                .ProjectTo<CourseReadDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            // Note: Depending on your design choice, you may want to return an empty list instead of a NotFound error when there are no courses.
            //if (courses.Count == 0)
            //{
            //    return Error.NotFound("No courses found.");
            //}
            return courses;
        }
        public async Task<Result<CourseReadDto>> GetCourseByIdAsync(int id)
        {   
            var course = await db.Courses
                .AsNoTracking()
                .Where(c => c.Id == id)
                .ProjectTo<CourseReadDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            if (course == null)
            {
                logger.LogWarning("Course with id {Id} not found.", id);
                return Error.NotFound($"Course with id {id} not found.");// implicit converstion to result.Fail(Error.NotFound($"Course with id {id} not found."));
            }
            return course; // returns Result.Ok(course) implicitly due to implicit conversion from CourseReadDTO to Result<CourseReadDTO>
        }
        public async Task<Result<CourseReadDto>> CreateCourseAsync(CourseCreateDto courseDto)
        {
            logger.LogInformation("Creating new course with title {Title}", courseDto.Title);

            if (courseDto == null)
            {
                logger.LogWarning("Create course failed: request body was null");
                return Error.Validation("Course data is required.");
            }
            var course = mapper.Map<Course>(courseDto);
            await db.Courses.AddAsync(course);
            await db.SaveChangesAsync();

            logger.LogInformation("Course with id {Id} and title '{Title}' created successfully.", course.Id, course.Title);

            return await db.Courses
                .AsNoTracking()
                .Where(c=>c.Id == course.Id)
                .ProjectTo<CourseReadDto>(mapper.ConfigurationProvider)
                .FirstAsync(); // to get the created course with the generated Id and CreatedAt
        }
        public async Task<Result<CourseReadDto>> UpdateCourseAsync(int id, CourseUpdateDto courseDto)
        {
            var course = await db.Courses.FindAsync(id);

            if (course == null)
            {
                logger.LogWarning("Attempted to update non-existing course with id {Id}", id);
                return Error.NotFound($"Course with id {id} not found.");
            }

            // Map DTO → existing entity
            mapper.Map(courseDto, course);

            await db.SaveChangesAsync();

            logger.LogInformation("Course with id {Id} and title '{Title}' updated successfully.", id, course.Title);

            return mapper.Map<CourseReadDto>(course);
        }

        public async Task<Result> DeleteCourseAsync(int id)
        {
            var course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                logger.LogWarning("Attempted to delete non-existing course with id {Id}", id);
                return Error.NotFound($"Course with id {id} not found.");
            }
            db.Courses.Remove(course);
            await db.SaveChangesAsync();
            logger.LogInformation("Course with id {Id} and title '{Title}' deleted successfully.", id, course.Title);
            return Result.Ok();
        }
    }
}
