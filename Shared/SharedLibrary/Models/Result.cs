namespace SharedLibrary.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public IEnumerable<string> Errors { get; }
        public T Value { get; set; }

        private Result(bool isSuccess, IEnumerable<string> errors, T value)
        {
            IsSuccess = isSuccess;
            Errors = errors;
            Value = value;
        }

        private Result(bool isSuccess, string error, T value)
        {
            IsSuccess = isSuccess;
            Errors = new[] { error };
            Value = value;
        }

        private Result(bool isSuccess, IEnumerable<FluentValidation.Results.ValidationFailure> errors, T value)
        {
            IsSuccess = isSuccess;
            Errors = errors.Select(p => p.ErrorMessage);
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(true, Array.Empty<string>(), value);
        public static Result<T> Failure(IEnumerable<string> errors) => new Result<T>(false, errors, default!);
        public static Result<T> Failure(string error) => new Result<T>(false, error, default!);
        public static Result<T> Failure(IEnumerable<FluentValidation.Results.ValidationFailure> errors) => new Result<T>(false, errors, default!);
    }
}
