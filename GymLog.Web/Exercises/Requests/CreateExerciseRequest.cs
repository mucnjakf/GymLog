using System.ComponentModel.DataAnnotations;
using GymLog.Web.Exercises.Dtos;

namespace GymLog.Web.Exercises.Requests;

public sealed record CreateExerciseRequest
{
    [Required] public string Name { get; set; } = string.Empty;

    [Required] public ExerciseCategory Category { get; set; }
};