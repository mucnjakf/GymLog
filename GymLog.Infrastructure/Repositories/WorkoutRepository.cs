using GymLog.Application.Workouts;
using GymLog.Domain.Workouts;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Infrastructure.Repositories;

internal sealed class WorkoutRepository : IWorkoutRepository
{
    private readonly ApplicationDbContext _dbContext;

    public WorkoutRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Workout>> GetAllWithExerciseAsync()
    {
        return await _dbContext
            .Set<Workout>()
            .Include(x => x.Exercise)
            .ToListAsync();
    }

    public async Task<IEnumerable<Workout>> GetWithExercisesAsync(DateTime dateTime)
    {
        return await _dbContext
            .Set<Workout>()
            .Include(x => x.Exercise)
            .Where(x => x.DateTime.Date == dateTime.Date)
            .ToListAsync();
    }

    public async Task<Workout?> GetAsync(Guid id)
    {
        return await _dbContext
            .Set<Workout>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Workout?> GetWithExerciseAsync(Guid id)
    {
        return await _dbContext
            .Set<Workout>()
            .Include(x => x.Exercise)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<DateTime>> GetDatesAsync()
    {
        List<DateTime> dates = await _dbContext
            .Set<Workout>()
            .Select(x => x.DateTime)
            .ToListAsync();

        return dates.DistinctBy(x => x.Date);
    }

    public void Insert(Workout workout)
    {
        _dbContext
            .Set<Workout>()
            .Add(workout);
    }

    public void Delete(Workout workout)
    {
        _dbContext
            .Set<Workout>()
            .Remove(workout);
    }
}