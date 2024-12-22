namespace PMSCore;

/// <summary>
/// Entity that holds all project data and it's state
/// </summary>
public class StateManager
{
    private StateManager(ILogger? logger)
    {
        _logger = logger ?? new LoggerFactory().CreateLogger("StateManager");
        _users = new();
        _entities = new();
        _userToEntityAssociation = new();
        _entityToEntityAssociation = new();
        _report = string.Empty;
    }

    /// <summary>
    /// Returns singleton instance for StateManager, creates a new instance if current on is null
    /// </summary>
    /// <returns></returns>
    public static StateManager GetInstance(ILogger? logger = null)
    {
        lock (_createLock)
        {
            if (_singleInstance == null)
            {
                _singleInstance = new StateManager(logger);
            }

            return _singleInstance;
        }
    }

    /// <summary>
    /// Resets singleton instance
    /// </summary>
    public static void ResetInstance()
    {
        lock (_createLock)
        {
            _singleInstance = null;
        }
    }

    /// <summary>
    /// Appends a given entity action to the entity report
    /// </summary>
    /// <param name="message"></param>
    private void AppendMessageToReport(string message)
    {
        _report += $"[StateManager:] {DateTime.Now} : {message}\n";
        _logger.LogInformation(message);
    }

    /// <summary>
    /// Gets StateManager report
    /// </summary>
    /// <returns></returns>
    public string GetReport() => _report;

    /// <summary>
    /// Internal validator for genetic object validation
    /// </summary>
    private enum ObjectType
    {
        Entity,
        User,
    }

    /// <summary>
    /// Internally validates if a given User or Entity exists
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="objectType"></param>
    /// <returns></returns>
    private EntityState FindObjectExists(string entityId, ObjectType objectType)
    {
        if (objectType == ObjectType.Entity)
        {
            return _entities.ContainsKey(entityId) ? EntityState.Ok : EntityState.NotFound;
        }

        return _users.ContainsKey(entityId) ? EntityState.Ok : EntityState.NotFound;
    }

    /// <summary>
    /// Checks if user exists for LoggingIn
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public EntityState UserLogin(string username)
    {
        if (FindObjectExists(username, ObjectType.User) != EntityState.Ok)
        {
            _logger.LogError($"User {username} not found");
            return EntityState.NotFound;
        }

        AppendMessageToReport($"User {username} is logged in.");
        _currentUser = _users[username];
        return EntityState.Ok;
    }

    /// <summary>
    /// Attempts to register user if it doesn't exist
    /// </summary>
    /// <param name="username"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public EntityState UserRegister(string username, IUser user)
    {
        if (!_users.TryAdd(username, user))
        {
            _logger.LogError($"User {username} already exists please login");
            return EntityState.AlreadyExists;
        }

        AppendMessageToReport($"User {username} registered.");
        _currentUser = _users[username];
        return EntityState.Ok;
    }

    /// <summary>
    /// Attempts to delete user
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public EntityState DeleteUser(string username)
    {
        if (FindObjectExists(username, ObjectType.User) != EntityState.Ok)
        {
            _logger.LogError($"Unable to delete {username}, user not found");
            return EntityState.NotFound;
        }

        _users.Remove(username);
        AppendMessageToReport($"User {username} deleted.");
        return EntityState.Ok;
    }

    /// <summary>
    /// Adds a new Entity entry
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public EntityState CreateEntity(Entity entity)
    {
        if (FindObjectExists(entity.GetId(), ObjectType.Entity) == EntityState.Ok)
        {
            _logger.LogWarning($"Entity {entity.GetId()} already exists");
            return EntityState.AlreadyExists;
        }

        _entities.Add(entity.GetId(), entity);
        AppendMessageToReport($"Entity {entity.GetId()} added");
        return EntityState.Ok;
    }

    /// <summary>
    /// Removes an Entity entry
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    public EntityState RemoveEntity(string entityId)
    {
        if (FindObjectExists(entityId, ObjectType.Entity) != EntityState.Ok)
        {
            _logger.LogWarning($"Entity {entityId} does not exist");
            return EntityState.NotFound;
        }

        _entities.Remove(entityId);
        AppendMessageToReport($"Entity {entityId} removed");
        return EntityState.Ok;
    }

    /// <summary>
    /// Gets an Entity Property
    /// </summary>
    /// <param name="id"></param>
    /// <param name="property"></param>
    /// <returns></returns>
    public (EntityState, object?) GetEntityProperty(string id, EntityProperty property)
    {
        AppendMessageToReport($"Attempting to fetch entity id: {id} for property {property}");
        if (FindObjectExists(id, ObjectType.Entity) != EntityState.Ok)
        {
            _logger.LogError($"Entity {id} does not exist");
            return (EntityState.NotFound, null);
        }

        if (property == EntityProperty.Description)
            return (EntityState.Ok, _entities[id].GetDescription());
        if (property == EntityProperty.EntityStatus)
            return (EntityState.Ok, _entities[id].GetStatus());
        if (property == EntityProperty.EntityPriority)
            return (EntityState.Ok, _entities[id].GetPriority());
        if (property == EntityProperty.CreatedDate)
            return (EntityState.Ok, _entities[id].GetCreatedDate());
        if (property == EntityProperty.StartedDate)
            return (EntityState.Ok, _entities[id].GetStartedDate());
        if (property == EntityProperty.FinishedDate)
            return (EntityState.Ok, _entities[id].GetFinishedDate());
        return (EntityState.Ok, _entities[id].GetReport());
    }

    /// <summary>
    /// Updates an entity property if it exists
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="property"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public EntityState UpdateEntityProperty(string entityId, EntityProperty property, object? value)
    {
        AppendMessageToReport(
            $"Attempting to update entity id: {entityId} with {value ?? "INVALID_PROPERTY"}, property {property}");
        if (value == null)
        {
            _logger.LogError($"Cannot update entity property {property} because value is null");
            return EntityState.Forbidden;
        }

        if (FindObjectExists(entityId, ObjectType.Entity) != EntityState.Ok)
        {
            _logger.LogError($"Entity {entityId} does not exist");
            return EntityState.NotFound;
        }

        switch (property)
        {
            case EntityProperty.Description:
                if (value is not string)
                {
                    _logger.LogError($"Cannot update entity property {property} because value is not a string");
                    return EntityState.Forbidden;
                }

                _entities[entityId].SetDescription((string)value);
                AppendMessageToReport($"Entity's {entityId}, {property} updated");
                return EntityState.Ok;

            case EntityProperty.EntityStatus:
                if (value is not EntityStatus)
                {
                    _logger.LogError($"Cannot update entity property {property} because value is not a EntityStatus");
                    return EntityState.Forbidden;
                }

                _entities[entityId].SetStatus((EntityStatus)value);
                AppendMessageToReport($"Entity's {entityId}, {property} updated");
                return EntityState.Ok;

            case EntityProperty.EntityPriority:
                if (value is not EntityPriority)
                {
                    _logger.LogError($"Cannot update entity property {property} because value is not a EntityPriority");
                    return EntityState.Forbidden;
                }

                _entities[entityId].SetPriority((EntityPriority)value);
                AppendMessageToReport($"Entity's {entityId}, {property} updated");
                return EntityState.Ok;

            case EntityProperty.StartedDate:
            case EntityProperty.FinishedDate:
                if (value is not DateTime)
                {
                    _logger.LogError($"Cannot update entity property {property} because value is not a DateTime");
                    return EntityState.Forbidden;
                }

                if (property == EntityProperty.StartedDate)
                    _entities[entityId].SetStartedDate((DateTime)value);
                if (property == EntityProperty.FinishedDate)
                    _entities[entityId].SetFinishedDate((DateTime)value);
                AppendMessageToReport($"Entity's {entityId}, {property} updated");
                return EntityState.Ok;
        }

        _logger.LogError($"Entity {entityId} not settable");
        return EntityState.Forbidden;
    }

    /// <summary>
    /// Updates a user property (userId | Permission)
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="property"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public EntityState UpdateUserProperty(string userId, UserProperty property, object value)
    {
        if (!_users.TryGetValue(userId, out var user))
        {
            _logger.LogError($"User {userId} does not exist");
            return EntityState.NotFound;
        }

        switch (property)
        {
            case UserProperty.ChangeUserId:
                if (value is not string)
                {
                    _logger.LogError($"Cannot update entity property {property} because value is not a string");
                    return EntityState.Forbidden;
                }

                return user.ChangeUserId((string)value) == UserOperationResult.Ok
                    ? EntityState.Ok
                    : EntityState.Forbidden;

            case UserProperty.AddPermissions or UserProperty.RemovePermissions:
                if (value is not Permission permission)
                {
                    _logger.LogError($"Cannot update entity property {property} because value is not a Permission");
                    return EntityState.Forbidden;
                }

                var removeUserPermission = property == UserProperty.AddPermissions
                    ? user.AddUserPermission(permission)
                    : user.RemoveUserPermission(permission);
                return removeUserPermission == UserOperationResult.Ok ? EntityState.Ok : EntityState.Forbidden;
        }

        return EntityState.Forbidden;
    }

    /// <summary>
    /// Returns a summary of user associations
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public (EntityState, string?) GetUserAssociations(string userId)
    {
        if (!_users.ContainsKey(userId))
        {
            _logger.LogError($"User {userId} does not exist");
            return (EntityState.NotFound, null);
        }

        if (!_userToEntityAssociation.ContainsKey(userId))
        {
            _logger.LogError($"User {userId} does not have associations");
            return (EntityState.NotFound, null);
        }

        string associationDetails = string.Empty;
        foreach (var entity in _userToEntityAssociation[userId])
        {
            associationDetails += $"[{userId} is associated with {entity.GetId()}\n";
        }

        return (EntityState.Ok, associationDetails);
    }

    /// <summary>
    /// Associates User to Entity
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public AssociationStatus AssociateUserToEntity(string entityId, string userId)
    {
        if (FindObjectExists(userId, ObjectType.User) != EntityState.Ok)
        {
            _logger.LogError($"User {userId} not found");
            return AssociationStatus.UserNotFound;
        }

        if (FindObjectExists(entityId, ObjectType.Entity) != EntityState.Ok)
        {
            _logger.LogError($"Entity {entityId} not found");
            return AssociationStatus.EntityNotFound;
        }

        if (_userToEntityAssociation.TryAdd(userId, new HashSet<Entity> { _entities[entityId] }))
        {
            AppendMessageToReport($"New User {userId} is associated to {entityId}");
            return AssociationStatus.NoError;
        }

        if (_userToEntityAssociation[userId].Contains(_entities[entityId]))
        {
            _logger.LogDebug($"User {userId} is already associated to entity {entityId}");
            return AssociationStatus.DuplicatedAssociation;
        }

        _userToEntityAssociation[userId].Add(_entities[entityId]);
        AppendMessageToReport($"User {userId} add associated to entity {entityId}");
        return AssociationStatus.NoError;
    }

    /// <summary>
    /// Disassociates User to Entity
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public AssociationStatus DisassociateUserToEntity(string entityId, string userId)
    {
        if (FindObjectExists(userId, ObjectType.User) != EntityState.Ok)
        {
            _logger.LogError($"User {userId} not found");
            return AssociationStatus.UserNotFound;
        }

        if (FindObjectExists(entityId, ObjectType.Entity) != EntityState.Ok)
        {
            _logger.LogError($"Entity {entityId} not found");
            return AssociationStatus.EntityNotFound;
        }

        if (!_userToEntityAssociation.ContainsKey(userId) ||
            _userToEntityAssociation[userId].Contains(_entities[entityId]))
        {
            _logger.LogError($"User {userId} did not have associated to entity {entityId}");
            return AssociationStatus.NoAssociation;
        }

        _userToEntityAssociation[userId].Remove(_entities[entityId]);
        AppendMessageToReport($"User {userId} remove associated from entity {entityId}");
        return AssociationStatus.NoError;
    }

    /// <summary>
    /// Internally associates project to task
    /// </summary>
    /// <param name="project"></param>
    /// <param name="task"></param>
    /// <returns></returns>
    private AssociationStatus AssociateEntities(Entity project, Entity task)
    {
        if (_entityToEntityAssociation.TryAdd(project.GetId(), new HashSet<Entity> { task }))
        {
            _logger.LogWarning($"New project {project.GetId()} is associated to task {task.GetId()}");
            return AssociationStatus.NoError;
        }

        if (_entityToEntityAssociation[project.GetId()].Contains(task))
        {
            _logger.LogWarning($"Project {project.GetId()} already associated to task {task.GetId()}");
            return AssociationStatus.DuplicatedAssociation;
        }

        _entityToEntityAssociation[project.GetId()].Add(task);
        AppendMessageToReport($"Project {project.GetId()} associated to task {task.GetId()}");
        return AssociationStatus.NoError;
    }

    /// <summary>
    /// Associates Entity to Entity
    /// </summary>
    /// <param name="entityA"></param>
    /// <param name="entityB"></param>
    /// <returns></returns>
    public AssociationStatus AssociateEntityToEntity(string entityA, string entityB)
    {
        if (FindObjectExists(entityA, ObjectType.Entity) != EntityState.Ok ||
            FindObjectExists(entityB, ObjectType.Entity) != EntityState.Ok)
        {
            _logger.LogError("One or both entities not found");
            return AssociationStatus.EntityNotFound;
        }

        if (_entities[entityA] is Project && _entities[entityB] is Task)
        {
            return AssociateEntities(_entities[entityA], _entities[entityB]);
        }

        if (_entities[entityA] is Task && _entities[entityB] is Project)
        {
            return AssociateEntities(_entities[entityB], _entities[entityA]);
        }

        _logger.LogError($"Cannot associate entity {entityA} to {entityB}");
        return AssociationStatus.InvalidAssociation;
    }

    /// <summary>
    /// Internally disassociates project to task
    /// </summary>
    /// <param name="project"></param>
    /// <param name="task"></param>
    /// <returns></returns>
    private AssociationStatus DisassociateEntities(Entity project, Entity task)
    {
        if (!_entityToEntityAssociation.ContainsKey(project.GetId()) ||
            !_entityToEntityAssociation[project.GetId()].Contains(task))
        {
            _logger.LogError($"Project {project.GetId()} is not associated to task {task.GetId()}");
            return AssociationStatus.NoAssociation;
        }

        _entityToEntityAssociation[project.GetId()].Remove(task);
        AppendMessageToReport($"Project {project.GetId()} removed association to task {task.GetId()}");
        return AssociationStatus.NoError;
    }

    /// <summary>
    /// Disassociates Entity from another Entity
    /// </summary>
    /// <param name="entityA"></param>
    /// <param name="entityB"></param>
    /// <returns></returns>
    public AssociationStatus DisassociateEntityToEntity(string entityA, string entityB)
    {
        if (FindObjectExists(entityA, ObjectType.Entity) != EntityState.Ok ||
            FindObjectExists(entityB, ObjectType.Entity) != EntityState.Ok)
        {
            _logger.LogError("One or both entities not found");
            return AssociationStatus.EntityNotFound;
        }

        if (_entities[entityA] is Project && _entities[entityB] is Task)
        {
            return DisassociateEntities(_entities[entityA], _entities[entityB]);
        }

        if (_entities[entityA] is Task && _entities[entityB] is Project)
        {
            return DisassociateEntities(_entities[entityB], _entities[entityA]);
        }

        _logger.LogError($"Cannot disassociate entity {entityA} to {entityB}");
        return AssociationStatus.InvalidAssociation;
    }

    private static StateManager? _singleInstance;
    private static ILogger _logger;
    private static Dictionary<string, IUser> _users;
    private static Dictionary<string, Entity> _entities;
    private static Dictionary<string, HashSet<Entity>> _userToEntityAssociation;
    private static Dictionary<string, HashSet<Entity>> _entityToEntityAssociation;
    private static IUser? _currentUser;
    private static string _report;
    private static readonly object _createLock = new();
    private readonly string DataStoragePath = "Data.json";
}