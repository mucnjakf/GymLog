using MediatR;

namespace GymLog.Application.Exercises.DeleteExercise;

public sealed record DeleteExerciseCommand(Guid Id) : IRequest;