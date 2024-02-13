namespace GymLog.Domain.Exceptions;

public sealed class ValidationException : Exception
{
    public IEnumerable<string> Errors { get; set; }

    public ValidationException(string message, IEnumerable<string> errors) : base(message)
    {
        Errors = errors;
    }
}