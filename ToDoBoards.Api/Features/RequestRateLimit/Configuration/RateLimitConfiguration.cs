namespace ToDoBoards.Api.Features.RequestRateLimit.Configuration;

public class RateLimitConfiguration
{
    public bool Enabled { get; set; }
    public int LimitPeriodDays { get; set; }
    public int LimitApiRequestsCount { get; set; } 
    public NotificationConfiguration Notification { get; set; }
}