using GymLog.Application.Aspects;
using MediatR;

namespace GymLog.Application.Workouts.GetWorkoutDates;

internal sealed class GetWorkoutDatesQueryHandler : IRequestHandler<GetWorkoutDatesQuery, WorkoutDatesDto>
{
    private readonly IWorkoutRepository _workoutRepository;

    public GetWorkoutDatesQueryHandler(IWorkoutRepository workoutRepository)
    {
        _workoutRepository = workoutRepository;
    }

    [Stopwatch]
    public async Task<WorkoutDatesDto> Handle(GetWorkoutDatesQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<DateTime> dates = await _workoutRepository.GetDatesAsync();
        
        WorkoutDatesDto workoutDatesDto = new(dates);

        return workoutDatesDto;
    }
}