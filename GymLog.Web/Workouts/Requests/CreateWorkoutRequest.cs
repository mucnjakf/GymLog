using System.ComponentModel.DataAnnotations;

namespace GymLog.Web.Workouts.Requests;

public sealed record CreateWorkoutRequest
{
    [Required] public string Duration { get; set; } = string.Empty;

    [Required] public DateTime DateTime { get; set; } = DateTime.UtcNow;

    [Required] public int Sets { get; set; }

    [Required] public int Reps { get; set; }

    [Required] public Guid ExerciseId { get; set; }
}