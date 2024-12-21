using System.Text.Json.Serialization;

namespace PMSCore;

[JsonConverter(typeof(UserJsonConverter))]
public abstract class User : IUser
{
    protected User(UserType userType, string userId, HashSet<Permission> userPermissions)
    {
        UserType = userType;
        UserId = userId;
        UserPermissions = userPermissions;
        UserReport = string.Empty;
    }

    private void AppendToReport(string message)
    {
        Console.WriteLine(message);
        UserReport += $"[{DateTime.UtcNow}] [{UserId}] [{message}]\n";
    }

    public string GetUserId() => UserId;

    public UserOperationResult CreateEntity(EntityType type, string entityId, string? entityDescription = null)
    {
        switch (type)
        {
            case EntityType.Project:
                if (CheckUserPermission(Permission.CanCreateProject) == UserOperationResult.Failed)
                {
                    AppendToReport("Not enough permissions to create a project");
                    return UserOperationResult.Failed;
                }

                break;
            case EntityType.Task:
                if (CheckUserPermission(Permission.CanCreateTask) == UserOperationResult.Failed)
                {
                    AppendToReport("Not enough permissions to create a task");
                    return UserOperationResult.Failed;
                }

                break;
        }

        AppendToReport($"Attempting to create {type.ToString()} [{entityId}]");
        var result = StateManager.GetInstance().CreateEntity(type, entityId, entityDescription);
        return result == EntityState.Ok ? UserOperationResult.Ok : UserOperationResult.Failed;
    }

    public UserOperationResult ChangeEntityProperty(EntityType type, string entityId, EntityProperty property,
        object? propertyValue)
    {
        switch (type)
        {
            case EntityType.Project:
                if (CheckUserPermission(Permission.CanChangeProjectProperty) == UserOperationResult.Failed)
                {
                    AppendToReport("Not enough permissions to change a project property");
                    return UserOperationResult.Failed;
                }

                break;
            case EntityType.Task:
                if (CheckUserPermission(Permission.CanChangeTaskProperty) == UserOperationResult.Failed)
                {
                    AppendToReport("Not enough permissions to change a project property");
                    return UserOperationResult.Failed;
                }

                break;
        }

        AppendToReport($"Attempting to change [{entityId}] property [{property}] value to [{propertyValue}]");

        return StateManager.GetInstance().UpdateEntityProperty(entityId, property, propertyValue) == EntityState.Ok
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public UserOperationResult DeleteEntity(EntityType type, string entityId)
    {
        switch (type)
        {
            case EntityType.Project:
                if (CheckUserPermission(Permission.CanDeleteProject) == UserOperationResult.Failed)
                {
                    AppendToReport("Not enough permissions to delete a project");
                    return UserOperationResult.Failed;
                }

                break;
            case EntityType.Task:
                if (CheckUserPermission(Permission.CanDeleteTask) == UserOperationResult.Failed)
                {
                    AppendToReport("Not enough permissions to delete a task");
                    return UserOperationResult.Failed;
                }

                break;
        }

        AppendToReport($"Attempting to delete entity with id: [{entityId}]");
        return StateManager.GetInstance().RemoveEntity(entityId) == EntityState.Ok
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public UserOperationResult AssociateEntityToEntity(string entityIdA, string entityIdB)
    {
        AppendToReport($"Attempting to associate entity [{entityIdA}] to entity [{entityIdB}]");
        return StateManager.GetInstance().AssociateEntityToEntity(entityIdA, entityIdB) == AssociationStatus.NoError
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public UserOperationResult DissociateEntityFromEntity(string entityIdA, string entityIdB)
    {
        AppendToReport($"Attempting to disassociate entity [{entityIdA}] to entity [{entityIdB}]");
        return StateManager.GetInstance().DisassociateEntityFromEntity(entityIdA, entityIdB) ==
               AssociationStatus.NoError
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public object GenerateEntityReport(string entityId)
    {
        if (UserOperationResult.Failed == CheckUserPermission(Permission.CanGenerateUserReport))
        {
            AppendToReport($"Can't generate user report for entity {entityId}");
            return (UserOperationResult.Failed);
        }

        AppendToReport($"Generating user report for entity [{entityId}]");
        var res = StateManager.GetInstance().GetEntityProperty(entityId, EntityProperty.Report);
        return (res.Item1 == EntityState.Ok ? res.Item2 as string : UserOperationResult.Failed)!;
    }

    public UserOperationResult DeleteUser(string userId)
    {
        if (UserOperationResult.Failed == CheckUserPermission(Permission.CanDeleteUser))
        {
            AppendToReport("Not enough permissions to delete a user");
            return UserOperationResult.Failed;
        }

        if (userId == UserId)
        {
            AppendToReport("Can't self delete user");
            return UserOperationResult.Failed;
        }

        AppendToReport($"Attempting to delete user [{userId}]");
        return StateManager.GetInstance().DeleteUser(userId) == EntityState.Ok
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public object GetUserAssociations(string userId)
    {
        AppendToReport($"Attempting to get user associations for user {userId}");
        var res = StateManager.GetInstance().GetUserAssociations(userId);
        if (res is string s)
        {
            return s;
        }

        return UserOperationResult.Failed;
    }

    public UserOperationResult AssociateUserWithEntity(string userId, string entityId)
    {
        AppendToReport($"Attempting to associate user [{userId}] to entity [{entityId}]");
        return StateManager.GetInstance().AssociateUserToEntity(entityId, userId) == AssociationStatus.NoError
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public UserOperationResult DisassociateUserWithEntity(string userId, string entityId)
    {
        AppendToReport($"Attempting to disassociate user [{userId}] from entity [{entityId}]");
        return StateManager.GetInstance().DisassociateUserFromEntity(entityId, userId) == AssociationStatus.NoError
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public UserOperationResult CheckUserPermission(Permission permission)
    {
        return UserPermissions.Contains(permission) ? UserOperationResult.Ok : UserOperationResult.Failed;
    }


    public UserOperationResult AddUserPermission(string userId, Permission permission)
    {
        if (CheckUserPermission(Permission.CanChangeUser) == UserOperationResult.Failed)
        {
            AppendToReport("Can't change user permission");
            return UserOperationResult.Failed;
        }

        AppendToReport($"Attempting to add permission {permission.ToString()} to ${userId}");
        return StateManager.GetInstance().UpdateUserProperty(userId, UserProperty.AddPermissions, permission) ==
               EntityState.Ok
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public UserOperationResult RemoveUserPermission(string userId, Permission permission)
    {
        if (CheckUserPermission(Permission.CanChangeUser) == UserOperationResult.Failed)
        {
            AppendToReport("Can't change user permission");
            return UserOperationResult.Failed;
        }

        AppendToReport($"Attempting to remove permission {permission.ToString()} from ${userId}");
        return StateManager.GetInstance().UpdateUserProperty(userId, UserProperty.RemovePermissions, permission) ==
               EntityState.Ok
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public UserOperationResult ChangeUserId(string userId, string newUserId)
    {
        if (CheckUserPermission(Permission.CanChangeUser) == UserOperationResult.Failed)
        {
            AppendToReport("Can't change user id");
            return UserOperationResult.Failed;
        }

        AppendToReport($"Attempting to change user id from {userId} to {newUserId}");
        return StateManager.GetInstance().UpdateUserProperty(userId, UserProperty.ChangeUserId, newUserId) ==
               EntityState.Ok
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public object GenerateUserReport(string userId)
    {
        if (CheckUserPermission(Permission.CanGenerateUserReport) == UserOperationResult.Failed)
        {
            AppendToReport("Can't change user id");
            return UserOperationResult.Failed;
        }

        return StateManager.GetInstance().GetUserReport(userId);
    }


    public UserType UserType { get; set; }
    public string UserId { get; set; }
    public HashSet<Permission> UserPermissions { get; set; }
    [JsonIgnore] public string UserReport { get; private set; }
}