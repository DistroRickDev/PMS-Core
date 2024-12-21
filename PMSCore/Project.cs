using System.Text.Json.Serialization;

namespace PMSCore;

/// <summary>
/// A project with tasks and deadlines.
/// </summary>
[JsonConverter(typeof(EntityJsonConverter))]
public class Project : Entity
{
    [JsonConstructor]
    public Project(string id, string? description, EntityStatus status, EntityPriority priority,
        DateTime createdDate, DateTime? startedDate = null, DateTime? finishedDate = null) : base(EntityType.Project, id,
        description, status, priority, createdDate, startedDate, finishedDate)
    {
    }

    /// <summary>
    /// Creates a project with an id, description
    /// </summary>
    private Project(string id, string? description) : base(EntityType.Project, id, description)
    {
        AppendMessageToReport($"Project: {id} created with description: {description ?? "No description"}");
    }

    /// <summary>
    ///  Project creator method
    /// </summary>
    /// <param name="callerLogger"></param>
    /// <param name="id"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public static Project? CreateProject(ILogger callerLogger, string? id, string? description = null)
    {
        if (!string.IsNullOrEmpty(id))
        {
            callerLogger.LogInformation(
                $"Created Project with {id}, and description: {description ?? "No description"}");
            return new Project(id, description);
        }

        callerLogger.LogInformation("Failed to create project reason: empty id");
        return null;
    }
}