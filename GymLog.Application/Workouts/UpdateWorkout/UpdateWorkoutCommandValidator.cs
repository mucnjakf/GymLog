using FluentValidation;

namespace GymLog.Application.Workouts.UpdateWorkout;

public sealed class UpdateWorkoutCommandValidator : AbstractValidator<UpdateWorkoutCommand>
{
    public UpdateWorkoutCommandValidator()
    {
        RuleFor(x => x.Duration)
            .NotEmpty().WithMessage("Duration is required.");

        RuleFor(x => x.DateTime)
            .NotEmpty().WithMessage("DateTime is required.");

        RuleFor(x => x.Sets)
            .GreaterThanOrEqualTo(0).WithMessage("Sets must be greater or equal to 0.");

        RuleFor(x => x.Reps)
            .GreaterThanOrEqualTo(0).WithMessage("Reps must be greater or equal to 0.");
    }
}