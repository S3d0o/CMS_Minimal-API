namespace CMS.Application.Validators
{
    public class StudentCerateOrUpdateValidator : AbstractValidator<StudentCreateOrUpdateDto>
    {
        public StudentCerateOrUpdateValidator()
        {
            RuleFor(x=> x.Name)
                .NotEmpty()
                .MaximumLength(100);
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotEmpty();
        }
    }
}
