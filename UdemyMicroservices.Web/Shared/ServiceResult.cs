namespace UdemyMicroservices.Web.Shared;

public class ServiceResult
{
    public bool IsSuccess =>
        string.IsNullOrEmpty(Error) && (ValidationErrors is null || ValidationErrors.Count == 0);

    public bool IsFail => !IsSuccess;

    public Dictionary<string, object>? ValidationErrors { get; init; }
    public string? Error { get; init; }


    // Static factory method for success
    public static ServiceResult Success()
    {
        return new ServiceResult();
    }

    // Static factory method for validation failure
    public static ServiceResult FailAsValidation(Dictionary<string, object> errors)
    {
        return new ServiceResult
        {
            ValidationErrors = errors
        };
    }

    // Static factory method for general failure
    public static ServiceResult Fail(string error)
    {
        return new ServiceResult
        {
            Error = error
        };
    }
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; init; }

    // Static factory method for success with data
    public static ServiceResult<T> Success(T data)
    {
        return new ServiceResult<T> { Data = data };
    }

    // Static factory method for validation failure
    public new static ServiceResult<T> FailAsValidation(Dictionary<string, object> errors)
    {
        return new ServiceResult<T>
        {
            ValidationErrors = errors
        };
    }

    // Static factory method for general failure
    public new static ServiceResult<T> Fail(string error)
    {
        return new ServiceResult<T>
        {
            Error = error
        };
    }
}