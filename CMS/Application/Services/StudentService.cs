
namespace CMS.Application.Services
{
    public class StudentService(CMS_DBContext db, IMapper mapper, ILogger<StudentService> logger) : IStudentService
    {
        public async Task<Result<StudentReadDto>> CreateAsync(StudentCreateOrUpdateDto studentCreateDto)
        {
            if (studentCreateDto == null)
            {
                logger.LogWarning("Student creation failed: input data is null");
                return Error.Validation("Student data is required.");
            }
            logger.LogInformation("Creating new student with name {Name} and email {Email}", studentCreateDto.Name, studentCreateDto.Email);

            
            var exists = await db.Students
                        .AnyAsync(s => s.Email == studentCreateDto.Email);

            if (exists)
            {
                logger.LogWarning("Student creation failed: email {Email} already exists", studentCreateDto.Email);
                return Error.Validation("Email already exists.");
            }

            var student = mapper.Map<Student>(studentCreateDto);
            await db.Students.AddAsync(student);
            await db.SaveChangesAsync();

            logger.LogInformation("Student created successfully with ID {Id}", student.Id);

            return mapper.Map<StudentReadDto>(student);
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var student = await db.Students.FindAsync(id);
            if (student == null)
            {
                logger.LogWarning("Student with ID {Id} not found for deletion", id);
                return Error.NotFound($"Student with ID {id} not found.");
            }
            var hasActiveEnrollments = await db.Enrollments
                .AnyAsync(e=>e.StudentId == id && e.Status == EnrollmentStatus.Active);
            if (hasActiveEnrollments)
            {
                logger.LogWarning("Student with ID {Id} cannot be deleted due to active enrollments", id);
                return Error.Validation("Cannot delete student with active enrollments.");
            }

            db.Students.Remove(student);
            await db.SaveChangesAsync();

            logger.LogInformation("Student with ID {Id} deleted successfully", id);

            return Result.Ok();
        }

        public async Task<Result<IEnumerable<StudentReadDto>>> GetAllAsync()
        {
            var students = await db.Students
                .AsNoTracking()
                .ProjectTo<StudentReadDto>(mapper.ConfigurationProvider)
                .ToListAsync();
            return students;
        }

        public async Task<Result<StudentReadDto>> GetByIdAsync(int id)
        {
            var student = await db.Students
                    .AsNoTracking()
                    .Where(s => s.Id == id)
                    .ProjectTo<StudentReadDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
            if (student == null)
            {
                logger.LogWarning("Student with ID {Id} not found", id);
                return Error.NotFound($"Student with ID {id} not found.");
            }

            return student;
        }

        public async Task<Result<StudentReadDto>> UpdateAsync(int id, StudentCreateOrUpdateDto studentUpdateDto)
        {
            var student = await db.Students.FindAsync(id);

            if (student == null)
            {
                logger.LogWarning("Student with ID {Id} not found", id);
                return Error.NotFound($"Student with ID {id} not found.");
            }

            var emailExists = await db.Students
                             .AnyAsync(s => s.Email == studentUpdateDto.Email && s.Id != id);

            if (emailExists)
            {
                return Error.Validation("Email already exists.");
            }

            mapper.Map(studentUpdateDto, student);
            await db.SaveChangesAsync();

            logger.LogInformation("Student with ID {Id} updated successfully", id);

            return mapper.Map<StudentReadDto>(student);
        }
    }
}
