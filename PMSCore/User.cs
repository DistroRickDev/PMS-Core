namespace PMSCore;

public abstract class User : IUser
{
    protected User(string userId, HashSet<Permission> userPermissions)
    {
        UserId = userId;
        UserPermissions = userPermissions;
        UserReport = string.Empty;
    }

    private void AppendToReport(string message)
    {
        Console.WriteLine(message);
        UserReport += $"[{DateTime.UtcNow}] [{UserId}] [{message}]\n";
    }

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

        AppendToReport($"Attempting to create entity [{entityId}]");
        var result = EntityManager.GetInstance().CreateEntity(entityId, entityDescription, type);
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
        EntityState entityState = property switch
        {
            EntityProperty.Description => EntityManager.GetInstance()
                .UpdateEntityDescription(entityId, propertyValue as string),
            EntityProperty.EntityStatus => EntityManager.GetInstance()
                .UpdateEntityStatus(entityId, propertyValue is EntityStatus value ? value : EntityStatus.New),
            EntityProperty.EntityPriority => EntityManager.GetInstance().UpdateEntityPriority(entityId,
                propertyValue is EntityPriority value ? value : EntityPriority.Lowest),
            EntityProperty.StartedDate => EntityManager.GetInstance()
                .UpdateEntityStartDate(entityId, propertyValue is DateTime date ? date : DateTime.MinValue),
            EntityProperty.FinishedDate => EntityManager.GetInstance()
                .UpdateEntityFinishDate(entityId, propertyValue is DateTime date ? date : DateTime.MinValue),
            _ => EntityState.Forbidden
        };
        return entityState == EntityState.Ok ? UserOperationResult.Ok : UserOperationResult.Failed;
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
        return EntityManager.GetInstance().RemoveEntity(entityId) == EntityState.Ok
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public UserOperationResult AssociateEntityToEntity(string entityIdA, string entityIdB)
    {
        AppendToReport($"Attempting to associate entity [{entityIdA}] to entity [{entityIdB}]");
        return EntityManager.GetInstance().AssociateEntityToEntity(entityIdA, entityIdB) == AssociationStatus.NoError
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public UserOperationResult DissociateEntityFromEntity(string entityIdA, string entityIdB)
    {
        AppendToReport($"Attempting to disassociate entity [{entityIdA}] to entity [{entityIdB}]");
        return EntityManager.GetInstance().DisassociateEntityFromEntity(entityIdA, entityIdB) ==
               AssociationStatus.NoError
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public (UserOperationResult, string?) GenerateEntityReport(string entityId)
    {
        if (UserOperationResult.Failed == CheckUserPermission(Permission.CanGenerateUserReport))
        {
            AppendToReport($"Can't generate user report for entity {entityId}");
            return (UserOperationResult.Failed, string.Empty);
        }

        AppendToReport($"Generating user report for entity [{entityId}]");
        var res = EntityManager.GetInstance().GetEntityReport(entityId);
        return res.Item1 == EntityState.Ok ? (UserOperationResult.Ok, res.Item2) : (UserOperationResult.Failed, null);
    }

    public UserOperationResult ChangeUserProperty(IUser requester, string userId, UserProperty property,
        object propertyValue)
    {
        if (UserOperationResult.Failed == requester.CheckUserPermission(Permission.CanChangeUser))
        {
            AppendToReport("Can't change user property");
            return UserOperationResult.Failed;
        }

        AppendToReport($"Attempting to change user property [{property}] value to [{propertyValue}]");
        return StateManager.GetInstance().UpdateUserProperty(userId, property, propertyValue) == EntityState.Ok
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
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

    public (UserOperationResult, string?) GetUserAssociations(string userId)
    {
        AppendToReport($"Attempting to get user associations for user {userId}");
        var res = StateManager.GetInstance().GetUserAssociations(userId);
        return res.Item1 == EntityState.Ok ? (UserOperationResult.Ok, res.Item2) : (UserOperationResult.Failed, null);
        ;
    }

    public UserOperationResult AssociateUserWithEntity(string userId, string entityId)
    {
        AppendToReport($"Attempting to associate user [{userId}] to entity [{entityId}]");
        return EntityManager.GetInstance().AssociateEntityToUser(entityId, userId) == AssociationStatus.NoError
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public UserOperationResult DisassociateUserWithEntity(string userId, string entityId)
    {
        AppendToReport($"Attempting to disassociate user [{userId}] from entity [{entityId}]");
        return EntityManager.GetInstance().DisassociateEntityFromUser(entityId, userId) == AssociationStatus.NoError
            ? UserOperationResult.Ok
            : UserOperationResult.Failed;
    }

    public UserOperationResult CheckUserPermission(Permission permission)
    {
        return UserPermissions.Contains(permission) ? UserOperationResult.Ok : UserOperationResult.Failed;
    }

    public UserOperationResult AddUserPermission(Permission permission)
    {
        if (CheckUserPermission(permission) == UserOperationResult.Ok)
        {
            AppendToReport($"User already contains permission {permission}");
            return UserOperationResult.Failed;
        }
        AppendToReport($"Attempting to add permission {permission}");
        UserPermissions.Add(permission);
        return UserOperationResult.Ok;
    }

    public UserOperationResult RemoveUserPermission(Permission permission)
    {
        if (CheckUserPermission(permission) == UserOperationResult.Failed)
        {
            AppendToReport($"User does not contain permission {permission}");
            return UserOperationResult.Failed;
        }
        AppendToReport($"Attempting to remove permission {permission}");
        UserPermissions.Remove(permission);
        return UserOperationResult.Ok;
    }

    public UserOperationResult ChangeUserId(string userId)
    {
        return userId == UserId ? UserOperationResult.Failed : UserOperationResult.Ok;
    }

    public (UserOperationResult, string?) GenerateUseReport(IUser requester, string entityId)
    {
        return UserOperationResult.Ok == requester.CheckUserPermission(Permission.CanGenerateUserReport)
            ? (UserOperationResult.Ok, UserReport)
            : (UserOperationResult.Failed, null);
    }

    public string UserId { get; private set; }
    public HashSet<Permission> UserPermissions { get; private set; }
    private string UserReport;
}