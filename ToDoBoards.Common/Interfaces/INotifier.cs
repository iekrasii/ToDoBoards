namespace ToDoBoards.Common.Interfaces;

/// <summary>
/// Represents service that sends notification via particular channel
/// </summary>
public interface INotifier
{
    /// <summary>
    /// Send notification
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <typeparam name="T"></typeparam>
    void Notify<T>(T message);
}