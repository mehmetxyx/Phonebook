namespace Shared.Common;

public class Result<T>
{
    public bool IsSuccess { get; private set; }
    public T? Value { get; private set; }
    public string? Message { get; private set; }
    public Result(bool isSuccess, T? value, string? message)
    {
        IsSuccess = isSuccess;
        Value = value;
        Message = message;
    }

    public static Result<T> Success(T value) => new Result<T>(true, value, "");
    public static Result<T> Success(string message) => new Result<T>(true, default!, message);
    public static Result<T> Success(T value, string message) => new Result<T>(true, value, message);

    public static Result<T> Failure(T value) => new Result<T>(false, value, "");
    public static Result<T> Failure(string message) => new Result<T>(false, default!, message);
    public static Result<T> Failure(T value, string message) => new Result<T>(false, value, message);
}