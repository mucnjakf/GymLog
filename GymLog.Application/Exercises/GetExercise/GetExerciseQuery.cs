using MediatR;

namespace GymLog.Application.Exercises.GetExercise;

public sealed record GetExerciseQuery(Guid Id) : IRequest<ExerciseDto>;