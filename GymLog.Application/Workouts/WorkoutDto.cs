using GymLog.Application.Aspects;

namespace GymLog.Application.Workouts;

[ToString]
public sealed class WorkoutDto
{
    public Guid Id { get; }

    public string Duration { get; }

    public string DateTime { get; }

    public int Sets { get; }

    public int Reps { get; }

    public WorkoutExerciseDto Exercise { get; }

    public WorkoutDto(Guid id, string duration, string dateTime, int sets, int reps, WorkoutExerciseDto exercise)
    {
        Id = id;
        Duration = duration;
        DateTime = dateTime;
        Sets = sets;
        Reps = reps;
        Exercise = exercise;
    }
}