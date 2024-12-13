namespace PMSCore;

/// <summary>
/// Enum representing possible errors on association between User and Project
/// </summary>
public enum AssociationStatus
{
    NoError = 0,
    InvalidUser,
    InvalidPermission,
    InvalidProject,
    InvalidTask,
    DuplicatedProjectToUserAssociation,
    UserNotFound,
    TaskNotFound,
    ProjectNotFound,
    UnknownError,
}