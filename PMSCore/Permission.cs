// Permission.cs
namespace PMSCore
{
    /// <summary>
    /// Represents a user's specific access permissions.
    /// Allows multiple permissions to be assigned to a single user.
    /// </summary>
    [Flags]
    public enum Permission
    {
        None = 0,
        CanCreateProject = 1,
        CanChangeProjectStatus = 2,
        CanCreateTask = 4,
        CanChangeTaskStatus = 8,
        CanCreateUser = 16,
        CanDeleteUser = 32,
        CanGenerateReport = 64
    }
}