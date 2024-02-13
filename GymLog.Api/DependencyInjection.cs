using Carter;
using GymLog.Api.Handlers;
using GymLog.Api.Health;
using GymLog.Application;
using GymLog.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Prometheus;

namespace GymLog.Api;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddCors(options => options.AddPolicy("Default", builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen();

        services
            .AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("database");

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddCarter();

        services
            .AddInfrastructure(configuration)
            .AddApplication();

        return services;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseCors("Default");

        app.UseExceptionHandler();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.ApplyMigrations();

        app.MapCarter();

        app.UseHttpsRedirection();

        app.MapHealthChecks("/api/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseMetricServer("/api/metrics");
        app.UseHttpMetrics();
        app.MapMetrics();

        return app;
    }

    private static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (dbContext.Database.IsRelational())
        {
            dbContext.Database.Migrate();
        }
    }
}