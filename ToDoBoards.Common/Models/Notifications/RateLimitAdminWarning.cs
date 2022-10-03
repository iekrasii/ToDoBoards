namespace ToDoBoards.Common.Models.Notifications;

public class RateLimitAdminWarning
{
    public int PercentageReached { get; set; }
    public string ApiPath { get; set; }

    public override string ToString()
    {
        return $"Admin be aware! API '{ApiPath}' has been used {PercentageReached}% of the rate limit.";
    }
}