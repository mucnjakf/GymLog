using System.ComponentModel.DataAnnotations;

namespace GymLog.Web.Workouts.Requests;

public sealed record UpdateWorkoutRequest
{
    [Required] public string Duration { get; set; } = default!;

    [Required] public DateTime DateTime { get; set; }

    [Required] public int Sets { get; set; }

    [Required] public int Reps { get; set; }

    [Required] public Guid ExerciseId { get; set; }
}