namespace GymLog.Domain.Exercises.Exceptions;

public sealed class ExerciseNotFoundException : Exception
{
    public ExerciseNotFoundException(string message) : base(message)
    {
    }
}