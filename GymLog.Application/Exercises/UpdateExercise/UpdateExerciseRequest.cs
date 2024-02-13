using GymLog.Domain.Exercises;

namespace GymLog.Application.Exercises.UpdateExercise;

public sealed record UpdateExerciseRequest(string Name, ExerciseCategory Category);