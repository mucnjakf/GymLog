using FluentValidation;
using FluentValidation.Results;
using GymLog.Application.Aspects;
using GymLog.Application.Data;
using GymLog.Domain.Exercises;
using MediatR;
using ValidationException = GymLog.Domain.Exceptions.ValidationException;

namespace GymLog.Application.Exercises.CreateExercise;

internal sealed class CreateExerciseCommandHandler : IRequestHandler<CreateExerciseCommand, ExerciseDto>
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateExerciseCommand> _validator;

    public CreateExerciseCommandHandler(IExerciseRepository exerciseRepository, IUnitOfWork unitOfWork, IValidator<CreateExerciseCommand> validator)
    {
        _exerciseRepository = exerciseRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    [Stopwatch]
    public async Task<ExerciseDto> Handle(CreateExerciseCommand command, CancellationToken cancellationToken)
    {
        ValidationResult result = await _validator.ValidateAsync(command, cancellationToken);

        if (!result.IsValid)
        {
            throw new ValidationException("CreateExerciseCommand is invalid.", result.Errors.Select(x => x.ErrorMessage));
        }

        Exercise exercise = Exercise.Create(command.Name, command.Category);

        _exerciseRepository.Insert(exercise);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        ExerciseDto exerciseDto = new(exercise.Id, exercise.Name, exercise.Category.ToString(), Enumerable.Empty<ExerciseWorkoutDto>());
        
        return exerciseDto;
    }
}