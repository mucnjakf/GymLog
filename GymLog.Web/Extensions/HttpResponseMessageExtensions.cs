using System.Net.Http.Json;
using GymLog.Web.Abstract;

namespace GymLog.Web.Extensions;

public static class HttpResponseMessageExtensions
{
    public static async Task EnsureSuccessAsync(this HttpResponseMessage httpResponseMessage)
    {
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            ErrorResponseDto errorResponseDto = (await httpResponseMessage.Content.ReadFromJsonAsync<ErrorResponseDto>())!;

            throw new Exception(errorResponseDto.Message);
        }
    }
}