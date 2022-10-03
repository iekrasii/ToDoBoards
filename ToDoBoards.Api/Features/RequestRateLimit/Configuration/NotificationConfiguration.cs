namespace ToDoBoards.Api.Features.RequestRateLimit.Configuration;

public class NotificationConfiguration
{
    public bool Enabled { get; set; }
    public int LimitPercentageForNotification { get; set; }
}