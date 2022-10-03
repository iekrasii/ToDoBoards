using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ToDoBoards.Common.Interfaces;
using ToDoBoards.Common.Models;
using ToDoBoards.Common.Utils;
using ToDoBoards.Storage.Extensions;

namespace ToDoBoards.Storage.Storage;

/// <inheritdoc />
internal class RateLimitStorage : IRateLimitStorage
{
    private readonly StorageDbContext _storageDbContext;
    private readonly ILogger<RateLimitStorage> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RateLimitStorage"/> class
    /// </summary>
    /// <param name="storageDbContext">The database context</param>
    /// <param name="logger">The logger</param>
    public RateLimitStorage(StorageDbContext storageDbContext, ILogger<RateLimitStorage> logger)
    {
        _storageDbContext = storageDbContext;
        _logger = logger;
    }

    /// <inheritdoc />
    public Task<RateLimit> GetRateLimitAsync(string apiPath, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(apiPath))
            throw new InvalidOperationException(nameof(apiPath));

        try
        {
            return this._storageDbContext.RateLimits
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApiPath == apiPath, cancellationToken);
        }
        catch (Exception ex)
        {
            ex.HandleStorageException(this._logger);
            return null;
        }
    }

    /// <inheritdoc />
    public Task CreateOrUpdateRateLimitAsync(RateLimit rateLimit, CancellationToken cancellationToken)
    {
        if (rateLimit == null)
            throw new ArgumentNullException(nameof(rateLimit));

        return rateLimit.Id == Guid.Empty
            ? this.CreateRateLimitAsync(rateLimit, cancellationToken)
            : this.UpdateRateLimitAsync(rateLimit, cancellationToken);
    }

    private async Task CreateRateLimitAsync(RateLimit rateLimit, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var created = DateTime.UtcNow;
        rateLimit.Id = id;
        rateLimit.Created = created;
        rateLimit.Updated = created;

        try
        {
            await this._storageDbContext.RateLimits.AddAsync(rateLimit, cancellationToken);
            await this._storageDbContext.SaveChangesAsync(cancellationToken);

            this._logger.LogInformation(LogEvent.StorageSuccess, $"Created {nameof(RateLimit)} with ID {id}");
        }
        catch (Exception ex)
        {
            ex.HandleStorageException(this._logger);
        }
    }

    private async Task UpdateRateLimitAsync(RateLimit rateLimit, CancellationToken cancellationToken)
    {
        try
        {
            var existentRateLimit = await this._storageDbContext.RateLimits.FindAsync(new object[] { rateLimit.Id }, cancellationToken);
            if (existentRateLimit == null)
                throw new InvalidOperationException($"No rate limit found with ID: {rateLimit.Id}");

            existentRateLimit.CountApiCalls = rateLimit.CountApiCalls;
            existentRateLimit.NotificationIsSent = rateLimit.NotificationIsSent;
            existentRateLimit.Updated = DateTime.UtcNow;

            this._storageDbContext.RateLimits.Update(existentRateLimit);
            await this._storageDbContext.SaveChangesAsync(cancellationToken);

            this._logger.LogInformation(LogEvent.StorageSuccess, $"Updated {nameof(RateLimit)} with ID {rateLimit.Id}");
        }
        catch (Exception ex)
        {
            ex.HandleStorageException(this._logger);
        }
    }
}