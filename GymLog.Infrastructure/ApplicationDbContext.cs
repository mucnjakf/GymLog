using GymLog.Domain.Exercises;
using GymLog.Domain.Workouts;
using Microsoft.EntityFrameworkCore;

namespace GymLog.Infrastructure;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<Exercise> Exercises { get; init; } = default!;

    public DbSet<Workout> Workouts { get; init; } = default!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}