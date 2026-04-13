namespace CMS.Application.Interfaces
{
    public interface IStudentService
    {
        public Task<Result<IEnumerable<StudentReadDto>>> GetAllAsync();
        public Task<Result<StudentReadDto>> GetByIdAsync(int id);
        public Task<Result<StudentReadDto>> CreateAsync(StudentCreateOrUpdateDto studentCreateDto);
        public Task<Result<StudentReadDto>> UpdateAsync(int id, StudentCreateOrUpdateDto studentUpdateDto);
        public Task<Result> DeleteAsync(int id);
    }
}
