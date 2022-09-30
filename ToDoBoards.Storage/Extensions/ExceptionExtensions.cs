using System;
using Microsoft.Extensions.Logging;
using ToDoBoards.Common.Exceptions;
using ToDoBoards.Common.Utils;

namespace ToDoBoards.Storage.Extensions;

public static class ExceptionExtensions
{
    /// <summary>
    /// Logs exception appropriately and wraps exception with <see cref="StorageException"/>
    /// </summary>
    /// <param name="ex">Exception thrown</param>
    /// <param name="logger">Storage logger</param>
    /// <typeparam name="T">Logger Storage type</typeparam>
    /// <exception cref="StorageException"></exception>
    internal static void HandleStorageException<T>(this Exception ex, ILogger<T> logger)
    {
        if (ex.GetType() == typeof(InvalidOperationException))
            logger.LogWarning(LogEvent.StorageValidationError, ex, ex.Message);
        else
            logger.LogError(LogEvent.StorageUnknownError, ex, "Unexpected error occurred in storage");

        throw new StorageException(ex);
    }
}