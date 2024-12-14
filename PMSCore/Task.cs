namespace PMSCore;

public class Task : Entity
{
    /// <summary>
    /// Creates a Task with an id, description
    /// </summary>
    private Task(string id, string? description) : base(id, description)
    {
        AppendMessageToReport($"Task: {id} created with description: {description ?? "No description"}");
    }
        
    /// <summary>
    ///  Task creator method
    /// </summary>
    /// <param name="callerLogger"></param>
    /// <param name="id"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public static Task? CreateTask(ILogger callerLogger, string? id, string? description = null)
    {
        if (!string.IsNullOrEmpty(id))
        {
            callerLogger.LogInformation($"Created Task with {id}, and description: {description ?? "No description"}");
            return new Task(id, description);
        }
        callerLogger.LogInformation("Failed to create Task reason: empty id");
        return null;
    }
}