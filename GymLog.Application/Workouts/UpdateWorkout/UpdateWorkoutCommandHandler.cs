using FluentValidation;
using FluentValidation.Results;
using GymLog.Application.Aspects;
using GymLog.Application.Data;
using GymLog.Application.Exercises;
using GymLog.Domain.Exercises;
using GymLog.Domain.Exercises.Exceptions;
using GymLog.Domain.Workouts;
using GymLog.Domain.Workouts.Exceptions;
using MediatR;
using ValidationException = GymLog.Domain.Exceptions.ValidationException;

namespace GymLog.Application.Workouts.UpdateWorkout;

internal sealed class UpdateWorkoutCommandHandler : IRequestHandler<UpdateWorkoutCommand>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateWorkoutCommand> _validator;

    public UpdateWorkoutCommandHandler(
        IWorkoutRepository workoutRepository,
        IExerciseRepository exerciseRepository,
        IUnitOfWork unitOfWork,
        IValidator<UpdateWorkoutCommand> validator)
    {
        _workoutRepository = workoutRepository;
        _exerciseRepository = exerciseRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    [Stopwatch]
    public async Task Handle(UpdateWorkoutCommand command, CancellationToken cancellationToken)
    {
        ValidationResult result = await _validator.ValidateAsync(command, cancellationToken);

        if (!result.IsValid)
        {
            throw new ValidationException("UpdateWorkoutCommand is invalid.", result.Errors.Select(x => x.ErrorMessage));
        }

        Exercise? exercise = await _exerciseRepository.GetAsync(command.ExerciseId);

        if (exercise is null)
        {
            throw new ExerciseNotFoundException($"Exercise with ID {command.ExerciseId} not found.");
        }

        Workout? workout = await _workoutRepository.GetAsync(command.Id);

        if (workout is null)
        {
            throw new WorkoutNotFoundException($"Workout with ID {command.Id} not found.");
        }

        workout.Update(command.Duration, command.DateTime, command.Sets, command.Reps, exercise.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}