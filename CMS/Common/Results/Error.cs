namespace CMS.Common.Results
{
    public sealed class Error
    {
        public string Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        // Factory methods for common error types
        public static Error Failure(
            string code = "General.Failure",
            string description = "A general error occurred while processing your request.")
            => new Error(code, description, ErrorType.Failure);

        public static Error Validation(
       string code = "Validation.Failure",
       string description = "Validation error occurred")
       => new(code, description, ErrorType.Validation);

        public static Error NotFound(
            string code = "Resource.NotFound",
            string description = "Requested resource was not found")
            => new(code, description, ErrorType.NotFound);

        public static Error Unauthorized(
            string code = "Auth.Unauthorized",
            string description = "Unauthorized access")
            => new(code, description, ErrorType.Unauthorized);

        public static Error Forbidden(
            string code = "Auth.Forbidden",
            string description = "Forbidden access")
            => new(code, description, ErrorType.Forbidden);

        public static Error InvalidCredentials(
            string code = "Auth.InvalidCredentials",
            string description = "Invalid credentials")
            => new(code, description, ErrorType.InvalidCredentials);
    }
}
