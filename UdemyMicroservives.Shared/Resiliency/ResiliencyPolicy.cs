using Polly;
using Polly.Extensions.Http;
using Polly.Wrap;

namespace UdemyMicroservices.Shared.Resiliency;

public static class ResiliencyPolicy
{
    public static AsyncPolicyWrap<HttpResponseMessage> AddResiliencyCombinePolicy()
    {
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var circuitBreakerPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1));

        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(10);


        return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);
    }
}