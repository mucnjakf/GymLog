using GymLog.Application.Aspects;

namespace GymLog.Application.Exercises;

[ToString]
public sealed class ExerciseWorkoutDto
{
    public Guid Id { get; }

    public string Duration { get; }

    public string DateTime { get; }

    public int Sets { get; }

    public int Reps { get; }

    public ExerciseWorkoutDto(Guid id, string duration, string dateTime, int sets, int reps)
    {
        Id = id;
        Duration = duration;
        DateTime = dateTime;
        Sets = sets;
        Reps = reps;
    }
}