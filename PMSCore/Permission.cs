using System.ComponentModel;

namespace PMSCore
{
    /// <summary>
    /// An enum representing a user's access permission levels.
    /// </summary>
    public enum Permission
    {
        [Description("Can create a project")] CanCreateProject = 0,

        [Description("Can change a project property")]
        CanChangeProjectProperty,
        [Description("Can delete a project")] CanDeleteProject,
        [Description("Can create a task")] CanCreateTask,

        [Description("Can change a task property")]
        CanChangeTaskProperty,
        [Description("Can delete a task")] CanDeleteTask,

        [Description("Can generate an entity report")]
        CanGenerateEntityReport,
        [Description("Can delete a user")] CanDeleteUser,

        [Description("Can change a user property")]
        CanChangeUser,

        [Description("Can generate a user report")]
        CanGenerateUserReport,
    }
}