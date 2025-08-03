using Shared.Common;

namespace Shared.Api.Common;
public static class ResultExtensions
{
    public static ApiResponse<T> ToApiResponse<T>(this Result<T> result)
    {
        return new ApiResponse<T>
        {
            IsSuccessful = result.IsSuccess,
            Data = result.Value,
            Message = result.Message,
            Errors = result.IsSuccess ? null : new List<string> { result.Message ?? "An error occurred." }
        };
    }
}
