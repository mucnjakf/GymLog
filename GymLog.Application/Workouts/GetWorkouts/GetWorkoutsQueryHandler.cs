using GymLog.Application.Aspects;
using GymLog.Domain.Workouts;
using MediatR;

namespace GymLog.Application.Workouts.GetWorkouts;

internal sealed class GetWorkoutsQueryHandler : IRequestHandler<GetWorkoutsQuery, IEnumerable<WorkoutDto>>
{
    private readonly IWorkoutRepository _workoutRepository;

    public GetWorkoutsQueryHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    [Stopwatch]
    public async Task<IEnumerable<WorkoutDto>> Handle(GetWorkoutsQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Workout> workouts = await _workoutRepository.GetWithExercisesAsync(query.DateTime);

        IEnumerable<WorkoutDto> workoutDtos = workouts.Select(workout => new WorkoutDto(
            workout.Id,
            workout.Duration,
            workout.DateTime.ToString("yyyy-MM-dd"),
            workout.Sets,
            workout.Reps,
            new WorkoutExerciseDto(
                workout.Exercise.Id,
                workout.Exercise.Name,
                workout.Exercise.Category.ToString())
        ));

        return workoutDtos;
    }
}