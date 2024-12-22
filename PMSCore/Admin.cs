namespace PMSCore;

/// <summary>
/// Represents an Admin user with the highest level of access and permissions.
/// </summary>
public class Admin : User
{
    /// <summary>
    /// Initializes an Admin with all permissions.
    /// </summary>
    public Admin(string userId)
        : base(userId, new HashSet<Permission>
        {
            Permission.CanCreateProject,
            Permission.CanChangeProjectProperty,
            Permission.CanDeleteProject,
            Permission.CanCreateTask,
            Permission.CanChangeTaskProperty,
            Permission.CanDeleteTask,
            Permission.CanGenerateEntityReport,
            Permission.CanGenerateUserReport,
            Permission.CanChangeUser,
            Permission.CanDeleteUser
        })
    {
    }

    // There are no additional methods for validation because permission checks are handled by the base class.
}