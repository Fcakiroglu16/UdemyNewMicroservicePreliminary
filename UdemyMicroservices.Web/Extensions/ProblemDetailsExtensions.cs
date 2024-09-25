using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace UdemyMicroservices.Web.Extensions
{
    public static class ProblemDetailsExtensions
    {
        // Extension method for logging ProblemDetails
        public static void LogProblemDetails(this ILogger logger, string jsonContent)
        {
            var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(jsonContent);

            if (problemDetails != null)
            {
                // Log important fields of ProblemDetails
                logger.LogError(
                    "Problem Details: Title: {Title}, Detail: {Detail}, Status: {Status}",
                    problemDetails.Title,
                    problemDetails.Detail,
                    problemDetails.Status);
            }
            else
            {
                logger.LogError("Failed to deserialize ProblemDetails from content: {Content}", jsonContent);
            }
        }
    }
}