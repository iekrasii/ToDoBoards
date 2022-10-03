using ToDoBoards.Common.Interfaces;

namespace ToDoBoards.Notification;

/// <summary>
/// Sends notification to Console
/// </summary>
public class ConsoleNotifier : INotifier
{
    /// <inheritdoc />
    public void Notify<T>(T message)
    {
        Console.WriteLine(message?.ToString());
    }
}