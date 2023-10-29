using HunterX.Trader.Common.Logging;

namespace HunterX.Trader.Infrastructure.Services;

public class ApiThrottler
{
    private readonly int maxRequests;
    private readonly TimeSpan perTimeSpan;

    private readonly SemaphoreSlim semaphore;

    public ApiThrottler(int maxRequests, TimeSpan perTimeSpan)
    {
        this.maxRequests = maxRequests;
        this.perTimeSpan = perTimeSpan;

        semaphore = new SemaphoreSlim(this.maxRequests, this.maxRequests);
    }

    public async Task<TReturn> ThrottledRequestAsync<TReturn>(Func<Task<TReturn>> apiCallAsync, CancellationToken cancellationToken)
    {
        if (semaphore.CurrentCount == 0)
        {
            Logger.Warning("API Throttler at 0. Waiting for release. Limited to {MaxRequests} Requests Per {TimeSpan}.", maxRequests, perTimeSpan);
        }

        await semaphore.WaitAsync(cancellationToken);

        try
        {
            return await apiCallAsync();
        }
        finally
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(perTimeSpan, cancellationToken);
                }
                finally
                {
                    semaphore.Release();
                }
            }, cancellationToken);
        }
    }
}
