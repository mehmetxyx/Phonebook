namespace Shared.Api.Common;

public class ApiResponse<T>
{
    public bool IsSuccessful { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
}
