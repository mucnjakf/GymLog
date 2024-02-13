using FluentValidation;
using FluentValidation.Results;
using GymLog.Application.Aspects;
using GymLog.Application.Data;
using GymLog.Domain.Exercises;
using GymLog.Domain.Exercises.Exceptions;
using MediatR;
using ValidationException = GymLog.Domain.Exceptions.ValidationException;

namespace GymLog.Application.Exercises.UpdateExercise;

internal sealed class UpdateExerciseCommandHandler : IRequestHandler<UpdateExerciseCommand>
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<UpdateExerciseCommand> _validator;

    public UpdateExerciseCommandHandler(IExerciseRepository exerciseRepository, IUnitOfWork unitOfWork, IValidator<UpdateExerciseCommand> validator)
    {
        _exerciseRepository = exerciseRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    [Stopwatch]
    public async Task Handle(UpdateExerciseCommand command, CancellationToken cancellationToken)
    {
        ValidationResult result = await _validator.ValidateAsync(command, cancellationToken);

        if (!result.IsValid)
        {
            throw new ValidationException("UpdateExerciseCommand is invalid.", result.Errors.Select(x => x.ErrorMessage));
        }

        Exercise? exercise = await _exerciseRepository.GetAsync(command.Id);

        if (exercise is null)
        {
            throw new ExerciseNotFoundException($"Exercise with ID {command.Id} not found.");
        }

        exercise.Update(command.Name, command.Category);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}