namespace CMS.Common.Results
{
    // No unnecessary usings to remove
    public class Result
    {
        protected readonly List<Error> _errors = new();

        public bool IsSuccess => _errors.Count == 0;
        public bool IsFailure => !IsSuccess;

        public IReadOnlyList<Error> Errors => _errors.AsReadOnly(); // expose errors as read-only to prevent external modification
        protected Result() { }

        protected Result(IEnumerable<Error> errors)
        {
            if (errors is null)
                throw new ArgumentNullException(nameof(errors));

            _errors.AddRange(errors);
        }

        protected Result(Error error)
        {
            if (error is null)
                throw new ArgumentNullException(nameof(error));

            _errors.Add(error);
        }

        public static Result Ok()
            => new();

        public static Result Fail(Error error)
            => new(error);

        public static Result Fail(IEnumerable<Error> errors)
            => new(errors);

        public static implicit operator Result(Error error)
            => Fail(error);

        public static implicit operator Result(List<Error> errors)
            => Fail(errors);
    }

    public class Result<T> : Result
    {
        private readonly T _value;
        public T Value => IsSuccess ?
            _value
            : throw new InvalidOperationException("Cannot access the value of a failed result.");
        private Result(T value) : base()
        {
            _value = value;
        }
        private Result(Error error) : base(error)
        {
            _value = default!;
        }
        private Result(IEnumerable<Error> errors) : base(errors)
        {
            _value = default!;
        }

        public static Result<T> Ok(T value)
        => new(value);

        public static new Result<T> Fail(Error error) // the new keyword is used to hide the base class method
            => new(error);

        public static new Result<T> Fail(IEnumerable<Error> errors)
            => new(errors);

        public static implicit operator Result<T>(T value) => Ok(value); // allows implicit conversion from T to Result<T>
        public static implicit operator Result<T>(Error error) => Fail(error);
        public static implicit operator Result<T>(List<Error> errors) => Fail(errors);
    }
}
