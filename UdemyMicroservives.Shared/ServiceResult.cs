using System.Net;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UdemyMicroservices.Shared;

public interface IRequestByServiceResult<T> : IRequest<ServiceResult<T>>;

public interface IRequestByServiceResult : IRequest<ServiceResult>;

public class ServiceResult
{
    [JsonIgnore] public HttpStatusCode Status { get; init; }
    public ProblemDetails? Fail { get; init; }

    [JsonIgnore] public bool IsSuccess => Fail is null;
    [JsonIgnore] public bool IsFail => !IsSuccess;

    // Static factory method for success with No Content
    public static ServiceResult SuccessAsNoContent() => new()
    {
        Status = HttpStatusCode.NoContent
    };

    // Static factory method for error with ProblemDetails
    public static ServiceResult Error(ProblemDetails problemDetails, HttpStatusCode statusCode) => new()
    {
        Status = statusCode,
        Fail = problemDetails
    };

    // Static factory method for error with custom title and detail
    public static ServiceResult Error(string title, string detail, HttpStatusCode statusCode) => new()
    {
        Status = statusCode,
        Fail = new ProblemDetails
        {
            Title = title,
            Detail = detail,
            Status = (int)statusCode
        }
    };

    // Static factory method for validation errors
    public static ServiceResult ValidationError(IDictionary<string, object?> errors) => new()
    {
        Status = HttpStatusCode.BadRequest,
        Fail = new ProblemDetails
        {
            Title = "Validation errors occurred",
            Detail = "See the errors property for details",
            Extensions = errors
        }
    };
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; init; }
    public string? UrlAsCreated { get; init; }

    // Static factory method for success as OK
    public static ServiceResult<T> SuccessAsOk(T data) => new ServiceResult<T>
    {
        Data = data,
        Status = HttpStatusCode.OK
    };

    // Static factory method for success as Created
    public static ServiceResult<T> SuccessAsCreated(T data, string urlAsCreated) => new ServiceResult<T>
    {
        Data = data,
        Status = HttpStatusCode.Created,
        UrlAsCreated = urlAsCreated
    };

    // Static factory method for error with ProblemDetails (inherits from base class)
    public new static ServiceResult<T> Error(ProblemDetails problemDetails, HttpStatusCode statusCode) =>
        new ServiceResult<T>
        {
            Status = statusCode,
            Fail = problemDetails
        };

    // Static factory method for error with custom title and detail (inherits from base class)
    public new static ServiceResult<T> Error(string title, string detail, HttpStatusCode statusCode) =>
        new ServiceResult<T>
        {
            Status = statusCode,
            Fail = new ProblemDetails
            {
                Title = title,
                Detail = detail,
                Status = (int)statusCode
            }
        };

    // Static factory method for validation errors (inherits from base class)
    public new static ServiceResult<T> ValidationError(IDictionary<string, object?> errors) => new ServiceResult<T>
    {
        Status = HttpStatusCode.BadRequest,
        Fail = new ProblemDetails
        {
            Title = "Validation errors occurred",
            Detail = "See the errors property for details",
            Extensions = errors
        }
    };
}