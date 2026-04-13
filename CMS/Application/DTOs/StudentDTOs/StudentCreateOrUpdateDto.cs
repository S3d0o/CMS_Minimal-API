
namespace CMS.Application.DTOs.StudentDtos
{
    public record StudentCreateOrUpdateDto
    {
        public string Name { get; init; } = string.Empty;
        [EmailAddress]
        public string Email { get; init; } = string.Empty;
    }
}
