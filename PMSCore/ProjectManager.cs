using System.Text.Json.Serialization;

namespace PMSCore;

/// <summary>
/// Represents a Project Manager user, with permissions to manage projects and tasks.
/// </summary>
[JsonConverter(typeof(UserJsonConverter))]
public class ProjectManager : User
{
    /// <summary>
    /// Initializes a Project Manager with both project and task permissions.
    /// </summary>
    public ProjectManager(string userId)
        : base(UserType.ProjectManager, userId, new HashSet<Permission>
        {
            Permission.CanCreateProject,
            Permission.CanChangeProjectProperty,
            Permission.CanDeleteProject,
            Permission.CanCreateTask,
            Permission.CanChangeTaskProperty,
            Permission.CanDeleteTask,
            Permission.CanGenerateEntityReport
        })
    {
    }
}