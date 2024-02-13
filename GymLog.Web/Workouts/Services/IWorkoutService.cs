using GymLog.Web.Workouts.Dtos;
using GymLog.Web.Workouts.Requests;

namespace GymLog.Web.Workouts.Services;

public interface IWorkoutService
{
    Task<IEnumerable<WorkoutDto>> GetWorkoutsAsync();

    Task<IEnumerable<WorkoutDto>> GetWorkoutsAsync(DateTime dateTime);

    Task<WorkoutDatesDto> GetWorkoutDatesAsync();

    Task CreateWorkoutAsync(CreateWorkoutRequest request);

    Task UpdateWorkoutAsync(Guid workoutId, UpdateWorkoutRequest request);

    Task DeleteWorkoutAsync(Guid workoutId);
}