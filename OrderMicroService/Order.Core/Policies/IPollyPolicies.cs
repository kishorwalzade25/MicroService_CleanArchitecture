using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Core.Policies
{
    public interface IPollyPolicies
    {
        IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount);
        IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(int handledEventsAllowedBeforeBreaking, TimeSpan durationOfBreak);
        IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(TimeSpan timeout);
    }
}
