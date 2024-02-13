using Carter;
using GymLog.Application.Exercises;
using GymLog.Application.Exercises.CreateExercise;
using GymLog.Application.Exercises.DeleteExercise;
using GymLog.Application.Exercises.GetAllExercises;
using GymLog.Application.Exercises.GetExercise;
using GymLog.Application.Exercises.UpdateExercise;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymLog.Api.Endpoints;

public sealed class ExerciseEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/exercises", HandleGetAllExercisesAsync);

        app.MapGet("api/exercises/{id:guid}", HandleGetExerciseAsync);

        app.MapPost("api/exercises", HandleCreateExerciseAsync);

        app.MapPut("api/exercises/{id:guid}", HandleUpdateExerciseAsync);

        app.MapDelete("api/exercises/{id:guid}", HandleDeleteExerciseAsync);
    }

    private static async Task<IResult> HandleGetAllExercisesAsync(HttpContext context, ISender sender)
    {
        GetAllExercisesQuery query = new();

        IEnumerable<ExerciseDto> exercises = await sender.Send(query);

        return Results.Ok(exercises);
    }

    private static async Task<IResult> HandleGetExerciseAsync(HttpContext context, [FromRoute] Guid id, ISender sender)
    {
        GetExerciseQuery query = new(id);

        ExerciseDto exercise = await sender.Send(query);

        return Results.Ok(exercise);
    }

    private static async Task<IResult> HandleCreateExerciseAsync(HttpContext context, [FromBody] CreateExerciseRequest request, ISender sender)
    {
        CreateExerciseCommand command = new(request.Name, request.Category);

        ExerciseDto exercise = await sender.Send(command);

        return Results.Created($"api/exercises/{exercise.Id}", exercise);
    }

    private static async Task<IResult> HandleUpdateExerciseAsync(
        HttpContext context,
        [FromRoute] Guid id,
        [FromBody] UpdateExerciseRequest request,
        ISender sender)
    {
        UpdateExerciseCommand command = new(id, request.Name, request.Category);

        await sender.Send(command);

        return Results.NoContent();
    }

    private static async Task<IResult> HandleDeleteExerciseAsync(HttpContext context, [FromRoute] Guid id, ISender sender)
    {
        DeleteExerciseCommand command = new(id);

        await sender.Send(command);

        return Results.NoContent();
    }
}