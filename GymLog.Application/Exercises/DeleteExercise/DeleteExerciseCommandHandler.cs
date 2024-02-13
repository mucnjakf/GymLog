using GymLog.Application.Aspects;
using GymLog.Application.Data;
using GymLog.Domain.Exercises;
using GymLog.Domain.Exercises.Exceptions;
using MediatR;

namespace GymLog.Application.Exercises.DeleteExercise;

internal sealed class DeleteExerciseCommandHandler : IRequestHandler<DeleteExerciseCommand>
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExerciseCommandHandler(IExerciseRepository exerciseRepository, IUnitOfWork unitOfWork)
    {
        _exerciseRepository = exerciseRepository;
        _unitOfWork = unitOfWork;
    }

    [Stopwatch]
    public async Task Handle(DeleteExerciseCommand command, CancellationToken cancellationToken)
    {
        Exercise? exercise = await _exerciseRepository.GetWithWorkoutsAsync(command.Id);

        if (exercise is null)
        {
            throw new ExerciseNotFoundException($"Exercise with ID {command.Id} not found.");
        }

        if (exercise.Workouts is not null && exercise.Workouts.Any())
        {
            throw new ExerciseContainsWorkoutsException("Unable to delete exercise that contains workouts.");
        }

        _exerciseRepository.Delete(exercise);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}