using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using Polly.Wrap;

namespace Kentico.Kontent.Management.Modules.ResiliencePolicy;

/// <summary>
/// Provides a default (fallback) retry policy for HTTP requests
/// </summary>
public class DefaultResiliencePolicyProvider : IResiliencePolicyProvider
{
    private readonly int _maxRetryAttempts;

    /// <summary>
    /// Creates a default retry policy provider with a maximum number of retry attempts.
    /// </summary>
    /// <param name="maxRetryAttempts">Maximum retry attempts for a request.</param>
    public DefaultResiliencePolicyProvider(int maxRetryAttempts)
    {
        _maxRetryAttempts = maxRetryAttempts;
    }

    /// <summary>
    /// Gets the default (fallback) retry policy for HTTP requests.
    /// </summary>
    public IAsyncPolicy<HttpResponseMessage> Policy => WrappedPolicy();

    private IAsyncPolicy<HttpResponseMessage> WrappedPolicy()
    {
        var defaultPolicy = Polly.Policy
            .HandleResult<HttpResponseMessage>(result => Enum.IsDefined(typeof(RetryHttpCode), (RetryHttpCode)result.StatusCode))
            .WaitAndRetryAsync(
                _maxRetryAttempts,
                retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt) * 100));

        var retryAfterPolicy = Polly.Policy
            .HandleResult<HttpResponseMessage>(result => result.StatusCode == (HttpStatusCode)429)
            .WaitAndRetryAsync(
            retryCount: 1,
            sleepDurationProvider: (retryCount, response, context) => {
                var retryAfter = response?.Result?.Headers?.RetryAfter?.Delta;

                return retryAfter.HasValue ? retryAfter.Value : TimeSpan.Zero;
            },
            onRetryAsync: (response, timespan, retryCount, context) => Task.CompletedTask);

        return Polly.Policy.WrapAsync(defaultPolicy, retryAfterPolicy);
    }
}
