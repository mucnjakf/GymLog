using FluentValidation;
using FluentValidation.Results;
using GymLog.Application.Aspects;
using GymLog.Application.Data;
using GymLog.Application.Exercises;
using GymLog.Domain.Exercises;
using GymLog.Domain.Exercises.Exceptions;
using GymLog.Domain.Workouts;
using MediatR;
using ValidationException = GymLog.Domain.Exceptions.ValidationException;

namespace GymLog.Application.Workouts.CreateWorkout;

internal sealed class CreateWorkoutCommandHandler : IRequestHandler<CreateWorkoutCommand, WorkoutDto>
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateWorkoutCommand> _validator;

    public CreateWorkoutCommandHandler(
        IWorkoutRepository workoutRepository,
        IExerciseRepository exerciseRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateWorkoutCommand> validator)
    {
        _workoutRepository = workoutRepository;
        _exerciseRepository = exerciseRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    [Stopwatch]
    public async Task<WorkoutDto> Handle(CreateWorkoutCommand command, CancellationToken cancellationToken)
    {
        ValidationResult result = await _validator.ValidateAsync(command, cancellationToken);

        if (!result.IsValid)
        {
            throw new ValidationException("CreateWorkoutCommand is invalid.", result.Errors.Select(x => x.ErrorMessage));
        }

        Exercise? exercise = await _exerciseRepository.GetAsync(command.ExerciseId);

        if (exercise is null)
        {
            throw new ExerciseNotFoundException($"Exercise with ID {command.ExerciseId} not found.");
        }

        Workout workout = Workout.Create(command.Duration, command.DateTime, command.Sets, command.Reps, exercise.Id);

        _workoutRepository.Insert(workout);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        WorkoutDto workoutDto = new(workout.Id, workout.Duration, workout.DateTime.ToString("yyyy-MM-dd"), workout.Sets, workout.Reps,
            new WorkoutExerciseDto(exercise.Id, exercise.Name, exercise.Category.ToString()));

        return workoutDto;
    }
}