using GymLog.Application.Aspects;
using GymLog.Domain.Workouts;
using GymLog.Domain.Workouts.Exceptions;
using MediatR;

namespace GymLog.Application.Workouts.GetWorkout;

internal sealed class GetWorkoutQueryHandler : IRequestHandler<GetWorkoutQuery, WorkoutDto>
{
    private readonly IWorkoutRepository _workoutRepository;

    public GetWorkoutQueryHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    [Stopwatch]
    public async Task<WorkoutDto> Handle(GetWorkoutQuery query, CancellationToken cancellationToken)
    {
        Workout? workout = await _workoutRepository.GetWithExerciseAsync(query.Id);

        if (workout is null)
        {
            throw new WorkoutNotFoundException($"Workout with ID {query.Id} not found.");
        }

        WorkoutDto workoutDto = new(
            workout.Id,
            workout.Duration,
            workout.DateTime.ToString("yyyy-MM-dd"),
            workout.Sets,
            workout.Reps,
            new WorkoutExerciseDto(
                workout.Exercise.Id,
                workout.Exercise.Name,
                workout.Exercise.Category.ToString())
        );

        return workoutDto;
    }
}