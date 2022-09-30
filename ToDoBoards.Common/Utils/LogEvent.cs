using Microsoft.Extensions.Logging;
using ToDoBoards.Common.Enums;

namespace ToDoBoards.Common.Utils;

public static class LogEvent
{
    public static EventId StorageSuccess => EventIdCreate(ToDoBoardEvent.STORAGE_SUCCESS);
    public static EventId StorageValidationError => EventIdCreate(ToDoBoardEvent.STORAGE_VALIDATION_ERROR);
    public static EventId StorageUnknownError => EventIdCreate(ToDoBoardEvent.STORAGE_UNKNOWN_ERROR);

    private static EventId EventIdCreate(ToDoBoardEvent toDoBoardEvent)
    {
        return new EventId((int)toDoBoardEvent, toDoBoardEvent.ToString());
    }
}