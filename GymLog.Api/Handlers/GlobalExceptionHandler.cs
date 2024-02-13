using System.Net;
using System.Net.Mime;
using System.Text.Json.Serialization;
using GymLog.Domain.Exceptions;
using GymLog.Domain.Exercises.Exceptions;
using GymLog.Domain.Workouts.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace GymLog.Api.Handlers;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        ErrorResponseDto errorResponse = GetErrorResponse(exception);

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        httpContext.Response.StatusCode = errorResponse.HttpStatusCode;

        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }

    private static ErrorResponseDto GetErrorResponse(Exception exception) => exception switch
    {
        ExerciseContainsWorkoutsException ecwex => new ErrorResponseDto
            { Message = ecwex.Message, HttpStatusCode = (int)HttpStatusCode.Conflict },

        ExerciseNotFoundException enfex => new ErrorResponseDto
            { Message = enfex.Message, HttpStatusCode = (int)HttpStatusCode.NotFound },

        WorkoutNotFoundException wnfex => new ErrorResponseDto
            { Message = wnfex.Message, HttpStatusCode = (int)HttpStatusCode.NotFound },

        ValidationException vex => new ErrorResponseDto
            { Message = vex.Message, ValidationErrors = vex.Errors, HttpStatusCode = (int)HttpStatusCode.BadRequest },

        _ => new ErrorResponseDto { Message = "Unknown error occured", HttpStatusCode = (int)HttpStatusCode.InternalServerError, }
    };
}

internal sealed record ErrorResponseDto
{
    public string Message { get; init; } = default!;

    public IEnumerable<string>? ValidationErrors { get; set; }

    [JsonIgnore] public int HttpStatusCode { get; init; }
}