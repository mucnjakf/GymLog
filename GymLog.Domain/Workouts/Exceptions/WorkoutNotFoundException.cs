namespace GymLog.Domain.Workouts.Exceptions;

public sealed class WorkoutNotFoundException : Exception
{
    public WorkoutNotFoundException(string message) : base(message)
    {
    }
}