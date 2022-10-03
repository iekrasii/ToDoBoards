using System.Threading;
using System.Threading.Tasks;
using ToDoBoards.Common.Models;

namespace ToDoBoards.Common.Interfaces;

/// <summary>
/// Represents storage operations on RateLimit model
/// </summary>
public interface IRateLimitStorage
{
    /// <summary>
    /// Returns RateLimit of particular API path if exists, otherwise null
    /// </summary>
    /// <param name="apiPath">API route</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>RateLimit of API path</returns>
    Task<RateLimit> GetRateLimitAsync(string apiPath, CancellationToken cancellationToken);

    /// <summary>
    /// Adds if not exist or updates existent RateLimit
    /// </summary>
    /// <param name="rateLimit">RateLimit to create of update</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task CreateOrUpdateRateLimitAsync(RateLimit rateLimit, CancellationToken cancellationToken);
}