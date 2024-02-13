using GymLog.Application.Data;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Infrastructure;

public class DbManager : IDbManager
{
    private readonly ApplicationDbContext _dbContext;

    public DbManager(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteHealthCheckAsync()
    {
        await _dbContext.Database.ExecuteSqlAsync($"SELECT 1");
    }
}