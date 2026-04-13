
namespace CMS.Application.DTOs.CourseDTOs
{
    public record CourseCreateDto
    {
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public Types CourseType { get; init; }
        public decimal Price { get; init; }
    }
}
