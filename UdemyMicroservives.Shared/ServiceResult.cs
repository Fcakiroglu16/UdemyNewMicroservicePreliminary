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
        [JsonIgnore] public int? Status { get; set; }
        [JsonIgnore] public string? UrlAsCreated { get; set; }

        public ProblemDetails? Fail { get; set; }


        // Static factory method
        public static ServiceResult<T> SuccessAsOk(T data) => new ServiceResult<T>
        {
            Data = data,
            Status = HttpStatusCode.OK.GetHashCode()
        };

        public static ServiceResult<T> SuccessAsCreated(T data, string urlAsCreated) => new ServiceResult<T>
        {
            Data = data,
            Status = HttpStatusCode.Created.GetHashCode(),
            UrlAsCreated = urlAsCreated
        };

        public static ServiceResult<T> Error(ProblemDetails problemDetails, HttpStatusCode statusCode) =>
            new ServiceResult<T>
            {
                Status = statusCode.GetHashCode(),
                Fail = problemDetails
            };


        public static ServiceResult<T> Error(string title, string detail, HttpStatusCode statusCode) =>
            new ServiceResult<T>
            {
                Status = statusCode.GetHashCode(),
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
                Status = HttpStatusCode.BadRequest.GetHashCode(),
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
        [JsonIgnore] public int? Status { get; set; }

        public ProblemDetails? Fail { get; set; }

        // Static factory method
        public static ServiceResult SuccessAsNoContent() => new ServiceResult
        {
            Status = HttpStatusCode.NoContent.GetHashCode()
        };


        public static ServiceResult Error(ProblemDetails problemDetails, HttpStatusCode statusCode) =>
            new ServiceResult
            {
                Status = statusCode.GetHashCode(),
                Fail = problemDetails
            };


        public static ServiceResult Error(string title, string detail, HttpStatusCode statusCode) =>
            new ServiceResult
            {
                Status = statusCode.GetHashCode(),
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
                Status = HttpStatusCode.BadRequest.GetHashCode(),
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