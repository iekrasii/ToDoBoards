namespace ToDoBoards.Common.Models;

/// <summary>
/// API request rate limit model
/// </summary>
public class RateLimit : BaseEntity
{
    /// <summary>
    /// Route of the API used as a key for rate limiting
    /// </summary>
    public string ApiPath { get; set; }

    /// <summary>
    /// Amount of API calls within time window 
    /// </summary>
    public int CountApiCalls { get; set; }

    /// <summary>
    /// Shows that admin has been notified on oncoming limit
    /// </summary>
    public bool NotificationIsSent { get; set; }
}