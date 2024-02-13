using MediatR;

namespace GymLog.Application.Exercises.GetAllExercises;

public sealed record GetAllExercisesQuery : IRequest<IEnumerable<ExerciseDto>>;