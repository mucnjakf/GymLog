using FluentValidation;

namespace GymLog.Application.Exercises.UpdateExercise;

public sealed class UpdateExerciseCommandValidator : AbstractValidator<UpdateExerciseCommand>
{
    public UpdateExerciseCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Category is invalid.");
    }
}