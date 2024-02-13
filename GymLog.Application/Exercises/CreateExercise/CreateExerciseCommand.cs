using GymLog.Domain.Exercises;
using MediatR;

namespace GymLog.Application.Exercises.CreateExercise;

public sealed record CreateExerciseCommand(string Name, ExerciseCategory Category) : IRequest<ExerciseDto>;