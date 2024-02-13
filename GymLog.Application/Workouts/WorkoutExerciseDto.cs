using GymLog.Application.Aspects;

namespace GymLog.Application.Workouts;

[ToString]
public sealed class WorkoutExerciseDto
{
    public Guid Id { get; }

    public string Name { get; }

    public string Category { get; }

    public WorkoutExerciseDto(Guid id, string name, string category)
    {
        Id = id;
        Name = name;
        Category = category;
    }
}