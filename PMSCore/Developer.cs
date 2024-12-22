namespace PMSCore;

/// <summary>
/// Represents a Developer user with specific permissions related to tasks.
/// </summary>
public class Developer : User
{
    /// <summary>
    /// Initializes a Developer with task-related permissions.
    /// </summary>
    public Developer(string userId)
        : base(userId, new HashSet<Permission>
        {
            Permission.CanCreateTask,
            Permission.CanChangeTaskProperty,
            Permission.CanDeleteTask,
            Permission.CanGenerateEntityReport
        })
    {
    }
}