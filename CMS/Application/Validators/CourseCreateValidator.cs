namespace CMS.Application.Validators
{
    public class CourseCreateValidator : AbstractValidator<CourseCreateDto>
    {
        public CourseCreateValidator()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0);
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(500);
            RuleFor(x => x.CourseType)
                .IsInEnum();
        }
    }
}
