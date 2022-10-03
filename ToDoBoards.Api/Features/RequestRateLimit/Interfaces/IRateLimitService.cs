using System.Threading;
using System.Threading.Tasks;

namespace ToDoBoards.Api.Features.RequestRateLimit.Interfaces;

/// <summary>
/// Represents service to limit amount of requests on particular API
/// </summary>
public interface IRateLimitService
{
    /// <summary>
    /// Checks whether limit of requests is reached
    /// </summary>
    /// <param name="apiPath">API path</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if limit is reached, otherwise False</returns>
    Task<bool> LimitIsReachedAsync(string apiPath, CancellationToken cancellationToken);
}