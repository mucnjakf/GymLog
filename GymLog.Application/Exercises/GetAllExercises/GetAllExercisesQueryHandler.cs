using GymLog.Application.Aspects;
using GymLog.Domain.Exercises;
using MediatR;

namespace GymLog.Application.Exercises.GetAllExercises;

internal sealed class GetAllExercisesQueryHandler : IRequestHandler<GetAllExercisesQuery, IEnumerable<ExerciseDto>>
{
    private readonly IExerciseRepository _exerciseRepository;

    public GetAllExercisesQueryHandler(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    [Stopwatch]
    public async Task<IEnumerable<ExerciseDto>> Handle(GetAllExercisesQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Exercise> exercises = await _exerciseRepository.GetAllWithWorkoutsAsync();

        IEnumerable<ExerciseDto> exerciseDtos = exercises.Select(exercise => new ExerciseDto(
            exercise.Id,
            exercise.Name,
            exercise.Category.ToString(),
            exercise.Workouts?.Select(workout => new ExerciseWorkoutDto(
                workout.Id,
                workout.Duration,
                workout.DateTime.ToString("yyyy-MM-dd"),
                workout.Sets,
                workout.Reps)) ?? Enumerable.Empty<ExerciseWorkoutDto>()));

        return exerciseDtos;
    }
}