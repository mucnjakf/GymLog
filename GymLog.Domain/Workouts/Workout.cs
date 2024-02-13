using GymLog.Domain.Abstractions;
using GymLog.Domain.Exercises;

namespace GymLog.Domain.Workouts;

public sealed class Workout : Entity
{
    public string Duration { get; private set; }

    public DateTime DateTime { get; private set; }

    public int Sets { get; private set; }

    public int Reps { get; private set; }

    public Guid ExerciseId { get; private set; }

    public Exercise Exercise { get; private set; } = default!;

    private Workout(Guid id, string duration, DateTime dateTime, int sets, int reps, Guid exerciseId) : base(id)
    {
        Duration = duration;
        DateTime = dateTime;
        Sets = sets;
        Reps = reps;
        ExerciseId = exerciseId;
    }

    public static Workout Create(string duration, DateTime dateTime, int sets, int reps, Guid exerciseId)
    {
        return new Workout(Guid.NewGuid(), duration, dateTime, sets, reps, exerciseId);
    }

    public void Update(string duration, DateTime dateTime, int sets, int reps, Guid exerciseId)
    {
        Duration = duration;
        DateTime = dateTime;
        Sets = sets;
        Reps = reps;
        ExerciseId = exerciseId;
    }

    public void AddExercise(Exercise exercise)
    {
        Exercise = exercise;
    }
}