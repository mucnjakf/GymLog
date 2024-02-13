using GymLog.Domain.Exercises;

namespace GymLog.Application.Exercises;

public interface IExerciseRepository
{
    Task<IEnumerable<Exercise>> GetAllWithWorkoutsAsync();

    Task<Exercise?> GetAsync(Guid id);

    Task<Exercise?> GetWithWorkoutsAsync(Guid id);

    void Insert(Exercise exercise);

    void Delete(Exercise exercise);
}