using Carter;
using GymLog.Application.Workouts;
using GymLog.Application.Workouts.CreateWorkout;
using GymLog.Application.Workouts.DeleteWorkout;
using GymLog.Application.Workouts.GetAllWorkouts;
using GymLog.Application.Workouts.GetWorkout;
using GymLog.Application.Workouts.GetWorkoutDates;
using GymLog.Application.Workouts.GetWorkouts;
using GymLog.Application.Workouts.UpdateWorkout;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymLog.Api.Endpoints;

public sealed class WorkoutEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/workouts", HandleGetAllWorkoutsAsync);

        app.MapGet("api/workouts/{dateTime:datetime}", HandleGetWorkoutsAsync);

        app.MapGet("api/workouts/{id:guid}", HandleGetWorkoutAsync);

        app.MapGet("api/workouts/dates", HandleGetWorkoutDatesAsync);

        app.MapPost("api/workouts", HandleCreateWorkoutAsync);

        app.MapPut("api/workouts/{id:guid}", HandleUpdateWorkoutAsync);

        app.MapDelete("api/workouts/{id:guid}", HandleDeleteWorkoutAsync);
    }

    private static async Task<IResult> HandleGetAllWorkoutsAsync(HttpContext context, ISender sender)
    {
        GetAllWorkoutsQuery query = new();

        IEnumerable<WorkoutDto> workouts = await sender.Send(query);

        return Results.Ok(workouts);
    }

    private static async Task<IResult> HandleGetWorkoutsAsync(HttpContext context, [FromRoute] DateTime dateTime, ISender sender)
    {
        GetWorkoutsQuery query = new(dateTime);

        IEnumerable<WorkoutDto> workouts = await sender.Send(query);

        return Results.Ok(workouts);
    }

    private static async Task<IResult> HandleGetWorkoutAsync(HttpContext context, [FromRoute] Guid id, ISender sender)
    {
        GetWorkoutQuery query = new(id);

        WorkoutDto workout = await sender.Send(query);

        return Results.Ok(workout);
    }

    private static async Task<IResult> HandleGetWorkoutDatesAsync(HttpContext context, ISender sender)
    {
        GetWorkoutDatesQuery query = new();

        WorkoutDatesDto dates = await sender.Send(query);

        return Results.Ok(dates);
    }

    private static async Task<IResult> HandleCreateWorkoutAsync(HttpContext context, [FromBody] CreateWorkoutRequest request, ISender sender)
    {
        CreateWorkoutCommand command = new(request.Duration, request.DateTime, request.Sets, request.Reps, request.ExerciseId);

        WorkoutDto workout = await sender.Send(command);

        return Results.Created($"api/workouts/{workout.Id}", workout);
    }

    private static async Task<IResult> HandleUpdateWorkoutAsync(
        HttpContext context,
        [FromRoute] Guid id,
        [FromBody] UpdateWorkoutRequest request,
        ISender sender)
    {
        UpdateWorkoutCommand command = new(id, request.Duration, request.DateTime, request.Sets, request.Reps, request.ExerciseId);

        await sender.Send(command);

        return Results.NoContent();
    }

    private static async Task<IResult> HandleDeleteWorkoutAsync(HttpContext context, [FromRoute] Guid id, ISender sender)
    {
        DeleteWorkoutCommand command = new(id);

        await sender.Send(command);

        return Results.NoContent();
    }
}