using GymLog.Application.Aspects;

namespace GymLog.Application.Exercises;

[ToString]
public sealed class ExerciseDto
{
    public Guid Id { get; }

    public string Name { get; }

    public string Category { get; }

    public IEnumerable<ExerciseWorkoutDto> Workouts { get; }

    public ExerciseDto(Guid id, string name, string category, IEnumerable<ExerciseWorkoutDto> workouts)
    {
        Id = id;
        Name = name;
        Category = category;
        Workouts = workouts;
    }
}