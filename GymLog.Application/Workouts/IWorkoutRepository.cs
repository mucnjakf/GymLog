using GymLog.Domain.Workouts;

namespace GymLog.Application.Workouts;

public interface IWorkoutRepository
{
    Task<IEnumerable<Workout>> GetAllWithExerciseAsync();

    Task<IEnumerable<Workout>> GetWithExercisesAsync(DateTime dateTime);

    Task<Workout?> GetAsync(Guid id);

    Task<Workout?> GetWithExerciseAsync(Guid id);

    Task<IEnumerable<DateTime>> GetDatesAsync();

    void Insert(Workout workout);

    void Delete(Workout workout);
}