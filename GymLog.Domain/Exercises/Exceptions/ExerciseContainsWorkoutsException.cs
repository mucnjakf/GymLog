namespace GymLog.Domain.Exercises.Exceptions;

public sealed class ExerciseContainsWorkoutsException : Exception
{
    public ExerciseContainsWorkoutsException(string message) : base(message)
    {
    }
}