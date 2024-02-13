using GymLog.Domain.Exercises;

namespace GymLog.Application.Exercises.CreateExercise;

public sealed record CreateExerciseRequest(string Name, ExerciseCategory Category);