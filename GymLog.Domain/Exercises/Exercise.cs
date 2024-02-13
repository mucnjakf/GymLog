using GymLog.Domain.Abstractions;
using GymLog.Domain.Workouts;

namespace GymLog.Domain.Exercises;

public sealed class Exercise : Entity
{
    public string Name { get; private set; }

    public ExerciseCategory Category { get; private set; }

    public IList<Workout>? Workouts { get; private set; }

    private Exercise(Guid id, string name, ExerciseCategory category) : base(id)
    {
        Name = name;
        Category = category;
    }

    public static Exercise Create(string name, ExerciseCategory category)
    {
        return new Exercise(Guid.NewGuid(), name, category);
    }

    public void Update(string name, ExerciseCategory category)
    {
        Name = name;
        Category = category;
    }

    public void AddWorkout(Workout workout)
    {
        Workouts ??= new List<Workout>();

        Workouts.Add(workout);
    }
}