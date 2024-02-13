using GymLog.Application.Exercises;
using GymLog.Domain.Exercises;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Infrastructure.Repositories;

internal sealed class ExerciseRepository : IExerciseRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ExerciseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Exercise>> GetAllWithWorkoutsAsync()
    {
        return await _dbContext
            .Set<Exercise>()
            .Include(x => x.Workouts)
            .ToListAsync();
    }

    public async Task<Exercise?> GetAsync(Guid id)
    {
        return await _dbContext
            .Set<Exercise>()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Exercise?> GetWithWorkoutsAsync(Guid id)
    {
        return await _dbContext
            .Set<Exercise>()
            .Include(x => x.Workouts)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public void Insert(Exercise exercise)
    {
        _dbContext
            .Set<Exercise>()
            .Add(exercise);
    }

    public void Delete(Exercise exercise)
    {
        _dbContext
            .Set<Exercise>()
            .Remove(exercise);
    }
}