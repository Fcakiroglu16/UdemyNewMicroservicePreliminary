using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace UdemyMicroservices.Shared
{
    public interface IRequestByServiceResult<T> : IRequest<ServiceResult<T>>;


    public class ServiceResult<T>
    {
        public T? Data { get; set; }
        [JsonIgnore] public HttpStatusCode Status { get; set; }
        [JsonIgnore] public string? UrlAsCreated { get; set; }


        [JsonIgnore] public bool IsSuccess => Fail is null;
        [JsonIgnore] public bool IsFail => !IsSuccess;

        public ProblemDetails? Fail { get; set; }

        // Static factory method
        public static ServiceResult<T> SuccessAsOk(T data) => new ServiceResult<T>
        {
            Data = data,
            Status = HttpStatusCode.OK
        };

        public static ServiceResult<T> SuccessAsCreated(T data, string urlAsCreated) => new ServiceResult<T>
        {
            Data = data,
            Status = HttpStatusCode.Created,
            UrlAsCreated = urlAsCreated
        };

        public static ServiceResult<T> Error(ProblemDetails problemDetails, HttpStatusCode statusCode) =>
            new ServiceResult<T>
            {
                Status = statusCode,
                Fail = problemDetails
            };


        public static ServiceResult<T> Error(string title, string detail, HttpStatusCode statusCode) =>
            new ServiceResult<T>
            {
                Status = statusCode,
                Fail = new ProblemDetails
                {
                    Title = title,
                    Detail = detail,
                    Status = statusCode.GetHashCode()
                }
            };

        //validation error
        public static ServiceResult<T> ValidationError(IDictionary<string, object?> errors)
        {
            return new ServiceResult<T>
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
    }

    public class ServiceResult
    {
        [JsonIgnore] public HttpStatusCode Status { get; set; }

        public ProblemDetails? Fail { get; set; }

        // Static factory method
        public static ServiceResult SuccessAsNoContent() => new ServiceResult
        {
            Status = HttpStatusCode.NoContent
        };


        public static ServiceResult Error(ProblemDetails problemDetails, HttpStatusCode statusCode) =>
            new ServiceResult
            {
                Status = statusCode,
                Fail = problemDetails
            };


        public static ServiceResult Error(string title, string detail, HttpStatusCode statusCode) =>
            new ServiceResult
            {
                Status = statusCode,
                Fail = new ProblemDetails
                {
                    Title = title,
                    Detail = detail,
                    Status = statusCode.GetHashCode()
                }
            };

        //validation error
        public static ServiceResult ValidationError(IDictionary<string, object?> errors)
        {
            return new ServiceResult
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
    }
}