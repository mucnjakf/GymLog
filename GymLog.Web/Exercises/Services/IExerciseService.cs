using GymLog.Web.Exercises.Dtos;
using GymLog.Web.Exercises.Requests;

namespace GymLog.Web.Exercises.Services;

public interface IExerciseService
{
    Task<IEnumerable<ExerciseDto>> GetExercisesAsync();

    Task CreateProjectAsync(CreateExerciseRequest request);

    Task UpdateExerciseAsync(Guid exerciseId, UpdateExerciseRequest request);
    
    Task DeleteExerciseAsync(Guid exerciseId);
}