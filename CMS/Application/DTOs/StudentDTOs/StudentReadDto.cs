
namespace CMS.Application.DTOs.StudentDtos
{
    public record StudentReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
