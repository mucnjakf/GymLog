using GymLog.Application.Aspects;
using GymLog.Application.Data;
using GymLog.Domain.Workouts;
using GymLog.Domain.Workouts.Exceptions;
using MediatR;

namespace GymLog.Application.Workouts.DeleteWorkout;

internal sealed class DeleteWorkoutCommandHandler : IRequestHandler<DeleteWorkoutCommand>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWorkoutCommandHandler(IWorkoutRepository workoutRepository, IUnitOfWork unitOfWork)
    {
        _workoutRepository = workoutRepository;
        _unitOfWork = unitOfWork;
    }
    
    [Stopwatch]
    public async Task Handle(DeleteWorkoutCommand command, CancellationToken cancellationToken)
    {
        Workout? workout = await _workoutRepository.GetAsync(command.Id);

        if (workout is null)
        {
            throw new WorkoutNotFoundException($"Workout with ID {command.Id} not found.");
        }

        _workoutRepository.Delete(workout);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}