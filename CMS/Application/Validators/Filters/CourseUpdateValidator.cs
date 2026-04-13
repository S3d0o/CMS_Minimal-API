namespace CMS.Application.Validators.Filters
{
    public class CourseUpdateValidator : AbstractValidator<CourseUpdateDto>
    {
        public CourseUpdateValidator()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Course title is required.")
                .MaximumLength(100).WithMessage("Course title cannot exceed 100 characters.");
            RuleFor(c => c.Description)
                .MaximumLength(500).WithMessage("Course description cannot exceed 500 characters.");
             RuleFor(c => c.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .LessThanOrEqualTo(1000).WithMessage("Price cannot exceed 100.");
        }
    }
}
