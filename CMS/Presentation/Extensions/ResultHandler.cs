
namespace CMS.Presentation.Extensions
{
    public static class ResultHandler
    {
        // Handle Result without value
        public static IResult Handle(Result result)
        {
            if (result.IsSuccess)
                return Results.NoContent();

            return HandleProblem(result.Errors);
        }

        // Handle Result with value
        public static IResult Handle<T>(Result<T> result)
        {
            if (result.IsSuccess)
                return Results.Ok(result.Value);

            return HandleProblem(result.Errors);
        }

        private static IResult HandleProblem(IReadOnlyList<Error> errors)
        {
            if (!errors.Any())
            {
                return Results.Problem(
                    title: "An unexpected error occurred",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }

            if (errors.All(e => e.Type == ErrorType.Validation))
                return HandleValidationProblem(errors);

            return HandleSingleError(errors[0]);
        }

        private static IResult HandleSingleError(Error error)
        {
            return Results.Problem(
                title: error.Code,
                detail: error.Description,
                type: error.Type.ToString(),
                statusCode: GetStatusCode(error.Type)
            );
        }

        private static int GetStatusCode(ErrorType type)
        {
            return type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.InvalidCredentials => StatusCodes.Status401Unauthorized,
                ErrorType.Failure => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };
        }

        private static IResult HandleValidationProblem(IReadOnlyList<Error> errors)
        {
            var dictionary = new Dictionary<string, string[]>();

            foreach (var error in errors)
            {
                if (!dictionary.ContainsKey(error.Code))
                    dictionary[error.Code] = new[] { error.Description };
                else
                    dictionary[error.Code] =
                        dictionary[error.Code].Append(error.Description).ToArray();
            }

            return Results.ValidationProblem(dictionary);
        }
 
    }
}

