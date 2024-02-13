using FluentValidation;

namespace GymLog.Application.Exercises.CreateExercise;

public sealed class CreateExerciseCommandValidator : AbstractValidator<CreateExerciseCommand>
{
    public CreateExerciseCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Category is invalid.");
    }
}