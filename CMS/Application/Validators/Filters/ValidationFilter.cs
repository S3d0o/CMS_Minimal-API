using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CMS.Application.Validators.Filters
{
    public class ValidationFilter : IEndpointFilter
    {
        private readonly ILogger<ValidationFilter> _logger;

        public ValidationFilter(ILogger<ValidationFilter> logger)
        {
            _logger = logger;
        }

        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next)
        {
            var validatorType = typeof(IValidator<>);

            foreach (var arg in context.Arguments)
            {
                if (arg == null) continue;

                var type = arg.GetType();

                if (type.IsPrimitive || type == typeof(string))
                    continue;

                _logger.LogDebug("Validating request model of type {ModelType}", type.Name);

                var validator = context.HttpContext.RequestServices
                    .GetService(validatorType.MakeGenericType(type));

                if (validator is not IValidator v)
                {
                    _logger.LogDebug("No validator found for {ModelType}", type.Name);
                    continue;
                }

                var result = await v.ValidateAsync(
                    new ValidationContext<object>(arg));

                if (!result.IsValid)
                {
                    var errors = result.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );

                    _logger.LogWarning(
                        "Validation failed for {ModelType}. Errors: {@Errors}",
                        type.Name,
                        errors
                    );

                    return Results.ValidationProblem(errors);
                }
            }

            return await next(context);
        }
    }
}