using GymLog.Application.Aspects;
using GymLog.Domain.Exercises;
using GymLog.Domain.Exercises.Exceptions;
using MediatR;

namespace GymLog.Application.Exercises.GetExercise;

internal sealed class GetExerciseQueryHandler : IRequestHandler<GetExerciseQuery, ExerciseDto>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetExerciseQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    [Stopwatch]
    public async Task<ExerciseDto> Handle(GetExerciseQuery query, CancellationToken cancellationToken)
    {
        Exercise? exercise = await _exerciseRepository.GetWithWorkoutsAsync(query.Id);

        if (exercise is null)
        {
            throw new ExerciseNotFoundException($"Exercise with ID {query.Id} not found.");
        }

        ExerciseDto exerciseDto = new(
            exercise.Id,
            exercise.Name,
            exercise.Category.ToString(),
            exercise.Workouts?.Select(x => new ExerciseWorkoutDto(
                x.Id,
                x.Duration,
                x.DateTime.ToString("yyyy-MM-dd"),
                x.Sets,
                x.Reps
            )) ?? Enumerable.Empty<ExerciseWorkoutDto>()
        );

        return exerciseDto;
    }
}