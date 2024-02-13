using System.ComponentModel.DataAnnotations;
using GymLog.Web.Exercises.Dtos;

namespace GymLog.Web.Exercises.Requests;

public sealed record UpdateExerciseRequest
{
    [Required] public string Name { get; set; } = default!;

    [Required] public ExerciseCategory Category { get; set; }
}