using GymLog.Domain.Workouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymLog.Infrastructure.Configurations;

public sealed class WorkoutEntityTypeConfiguration : IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.ToTable("Workouts");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Duration)
            .IsRequired();
        
        builder
            .Property(x => x.DateTime)
            .IsRequired();
        
        builder
            .Property(x => x.Sets)
            .IsRequired();
        
        builder
            .Property(x => x.Reps)
            .IsRequired();
    }
}