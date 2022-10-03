using Microsoft.Extensions.Logging;
using ToDoBoards.Common.Enums;

namespace ToDoBoards.Common.Utils;

public static class LogEvent
{
    public static EventId StorageSuccess => EventIdCreate(StoragEvent.STORAGE_SUCCESS);
    public static EventId StorageValidationError => EventIdCreate(StoragEvent.STORAGE_VALIDATION_ERROR);
    public static EventId StorageUnknownError => EventIdCreate(StoragEvent.STORAGE_UNKNOWN_ERROR);

    private static EventId EventIdCreate(StoragEvent storagEvent)
    {
        return new EventId((int)storagEvent, storagEvent.ToString());
    }
}