namespace CRMSystem.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string ErrorCode { get; }

    protected Result(bool isSuccess, string errorCode)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
    }

    public static Result Success() =>
        new(true, string.Empty);

    public static Result Failure(string errorCode) =>
        new(false, errorCode);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string errorCode)
        : base(isSuccess, errorCode)
    {
        Value = value;
    }

    public static Result<T> Success(T value) =>
        new(true, value, string.Empty);

    public new static Result<T> Failure(string errorCode) =>
        new(false, default, errorCode);
}