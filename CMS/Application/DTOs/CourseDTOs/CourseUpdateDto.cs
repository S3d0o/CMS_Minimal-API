
namespace CMS.Application.DTOs.CourseDTOs
{
    public record CourseUpdateDto
    {
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
    }
}
