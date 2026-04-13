
namespace CMS.Application.Services
{
    public class EnrollmentService(CMS_DBContext db, IMapper mapper, ILogger<EnrollmentService> logger) : IEnrollmentService
    {
        public async Task<Result> EnrollAsync(int studentId, int courseId)
        {
            // validate if student exist
            var studentExist = await db.Students.AnyAsync(s => s.Id == studentId);
            if (!studentExist)
            {
                logger.LogWarning("Student with ID {StudentId} does not exist.", studentId);
                return Error.NotFound($"Student with ID {studentId} does not exist.");
            }

            //validate if course exist
            var courseExist = await db.Courses.AnyAsync(c => c.Id == courseId);
            if (!courseExist)
            {
                logger.LogWarning("Course with ID {CourseId} does not exist.", courseId);
                return Error.NotFound($"Course with ID {courseId} does not exist.");
            }

            logger.LogInformation("Enrolling student {StudentId} into course {CourseId}", studentId, courseId);


            var alreadyEnrolled = await db.Enrollments.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId);
            if (alreadyEnrolled)
            {
                logger.LogWarning("Student {StudentId} is already enrolled in course {CourseId}.", studentId, courseId);
                return Error.Validation($"Student with ID {studentId} is already enrolled in course with ID {courseId}.");
            }
            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = courseId,
                // staus is by default active when created
                // enrolledDate is by default now when created from the database
            };
            try
            {
                await db.Enrollments.AddAsync(enrollment);
                await db.SaveChangesAsync();
                logger.LogInformation("Student {StudentId} successfully enrolled in course {CourseId}.", studentId, courseId);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Duplicate enrollment detected.");
                return Error.Validation("Student is already enrolled in this course.");
            }

            return Result.Ok();
        }
        public async Task<Result> CancelEnrollmentAsync(int studentId, int courseId)
        {
            var enrollment = await db.Enrollments.FirstOrDefaultAsync(e => e.StudentId == studentId
                                                        && e.CourseId == courseId
                                                        && e.Status == EnrollmentStatus.Active);
            if (enrollment == null)
            {
                logger.LogWarning("No active enrollment found for student {StudentId} in course {CourseId}.", studentId, courseId);
                return Error.NotFound($"No active enrollment found for student with ID {studentId} in course with ID {courseId}.");
            }
            enrollment.Status = EnrollmentStatus.Cancelled;
            await db.SaveChangesAsync();
            logger.LogInformation("Enrollment for student {StudentId} in course {CourseId} has been cancelled.", studentId, courseId);
            return Result.Ok();
        }

        public async Task<Result> CompleteEnrollmentAsync(int studentId, int courseId)
        {
            var enrollment = await db.Enrollments.FirstOrDefaultAsync(e => e.StudentId == studentId
                                                        && e.CourseId == courseId
                                                        && e.Status == EnrollmentStatus.Active);
            if (enrollment == null)
            {
                logger.LogWarning("No active enrollment found for student {StudentId} in course {CourseId}.", studentId, courseId);
                return Error.NotFound($"No active enrollment found for student with ID {studentId} in course with ID {courseId}.");
            }
            enrollment.Status = EnrollmentStatus.Completed;
            await db.SaveChangesAsync();
            logger.LogInformation("Enrollment for student {StudentId} in course {CourseId} has been completed.", studentId, courseId);
            return Result.Ok();
        }

        public async Task<Result> DropEnrollmentAsync(int studentId, int courseId)
        {
            var enrollment = await db.Enrollments
                            .FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId);

            if (enrollment == null)
            {
                logger.LogWarning("Enrollment not found for student {StudentId} in course {CourseId}.", studentId, courseId);
                return Error.NotFound("Enrollment not found.");
            }
            if (enrollment.Status != EnrollmentStatus.Active)
            {
                logger.LogWarning("Only active enrollments can be dropped for student {StudentId} in course {CourseId}.", studentId, courseId);
                return Error.Validation("Only active enrollments can be dropped.");
            }
            enrollment.Status = EnrollmentStatus.Dropped;
            await db.SaveChangesAsync();

            logger.LogInformation("Enrollment for student {StudentId} in course {CourseId} has been dropped.", studentId, courseId);
            return Result.Ok();
        }


        public async Task<Result<IEnumerable<CourseReadDto>>> GetCoursesByStudentIdAsync(int studentId, EnrollmentStatus? status = null)
        {
         var existstudent =  await db.Students.AnyAsync(s => s.Id == studentId);
            if (!existstudent)
            {
                logger.LogWarning("Student with ID {StudentId} does not exist.", studentId);
                return Error.NotFound($"Student with ID {studentId} does not exist.");
            }
            var query = db.Enrollments
             .Where(e => e.StudentId == studentId);

            if (status.HasValue)
            {
                query = query.Where(e => e.Status == status.Value);
            }

            var courses = await query
                .Select(e => e.Course)
                .AsNoTracking()
                .ProjectTo<CourseReadDto>(mapper.ConfigurationProvider)
                .ToListAsync();

            return courses;
        }

        public async Task<Result<IEnumerable<StudentReadDto>>> GetStudentsByCourseIdAsync(int courseId)
        {
            var existcourse = await db.Courses.AnyAsync(c => c.Id == courseId);
            if (!existcourse)
            {
                logger.LogWarning("Course with ID {CourseId} does not exist.", courseId);
                return Error.NotFound($"Course with ID {courseId} does not exist.");
            }
            var students = await db.Enrollments
                .Where(e=>e.CourseId == courseId && e.Status == EnrollmentStatus.Active)
                .Select(e=>e.Student)
                .AsNoTracking()
                .ProjectTo<StudentReadDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            
            return students;
        }
    }
}
