using GymLog.Application.Aspects;
using GymLog.Domain.Workouts;
using MediatR;

namespace GymLog.Application.Workouts.GetAllWorkouts;

internal sealed class GetAllWorkoutsQueryHandler : IRequestHandler<GetAllWorkoutsQuery, IEnumerable<WorkoutDto>>
{
    private readonly IWorkoutRepository _workoutRepository;

    public GetAllWorkoutsQueryHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }
    
    [Stopwatch]
    public async Task<IEnumerable<WorkoutDto>> Handle(GetAllWorkoutsQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Workout> workouts = await _workoutRepository.GetAllWithExerciseAsync();

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