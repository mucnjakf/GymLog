using GymLog.Domain.Exercises;
using MediatR;

namespace GymLog.Application.Exercises.UpdateExercise;

public sealed record UpdateExerciseCommand(Guid Id, string Name, ExerciseCategory Category) : IRequest;