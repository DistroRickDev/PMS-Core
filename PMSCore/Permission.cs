namespace PMSCore
{
    /// <summary>
    /// Represents a user's specific access permissions.
    /// Allows multiple permissions to be assigned to a single user.
    /// </summary>
    [Flags]
    public enum Permission
    {
        None = 0,                // No permissions
        CanCreateProject = 1,    // Permission to create projects
        CanChangeProjectStatus = 2, // Permission to update project status
        CanCreateTask = 4,       // Permission to create tasks
        CanChangeTaskStatus = 8, // Permission to update task status
        CanCreateUser = 16,      // Permission to create users
        CanDeleteUser = 32,      // Permission to delete users
        CanGenerateReport = 64,  // Permission to generate reports
    }

    /// <summary>
    /// Utility methods for working with permissions.
    /// </summary>
    public static class PermissionHelper
    {
        /// <summary>
        /// Checks if the user has a specific permission.
        /// </summary>
        /// <param name="userPermission">The permissions assigned to the user.</param>
        /// <param name="requiredPermission">The permission to check for.</param>
        /// <returns>True if the user has the required permission, false otherwise.</returns>
        public static bool HasPermission(Permission userPermission, Permission requiredPermission)
        {
            return (userPermission & requiredPermission) == requiredPermission;
        }

        /// <summary>
        /// Adds a new permission to the user.
        /// </summary>
        /// <param name="userPermission">The current permissions of the user.</param>
        /// <param name="newPermission">The permission to add.</param>
        /// <returns>The updated permissions with the new permission added.</returns>
        public static Permission AddPermission(Permission userPermission, Permission newPermission)
        {
            return userPermission | newPermission;
        }

        /// <summary>
        /// Removes a specific permission from the user.
        /// </summary>
        /// <param name="userPermission">The current permissions of the user.</param>
        /// <param name="removePermission">The permission to remove.</param>
        /// <returns>The updated permissions with the specific permission removed.</returns>
        public static Permission RemovePermission(Permission userPermission, Permission removePermission)
        {
            return userPermission & ~removePermission;
        }
    }
}