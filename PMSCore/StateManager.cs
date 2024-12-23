using System.Text.Json;

namespace PMSCore;

/// <summary>
/// Entity that holds all project data and it's state
/// </summary>
public class StateManager
{
    private StateManager(ILogger? logger)
    {
        SetPersistencePath();
        _logger = logger ?? LoggerFactory
            .Create(builder =>
                builder.AddProvider(new PmsLoggerProvider(Path.Combine(PersistencePath!, "PMSCore-StateManager.log"))))
            .CreateLogger("StateManager");
        LoadFromPersistence();
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
            return _singleInstance ??= new StateManager(logger);
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

    public EntityState UserLogout()
    {
        if (_currentUser == null)
        {
            AppendMessageToReport("User not logged in");
            return EntityState.NotFound;
        }

        AppendMessageToReport($"Logging out of user {_currentUser.GetUserId()}");
        _currentUser = null;
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
        if (!_users.TryAdd(username, (User)user))
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
    /// <param name="entityType"></param>
    /// <param name="entityId"></param>
    /// <param name="description"></param>
    /// <returns></returns>
    public EntityState CreateEntity(EntityType entityType, string entityId, string? description = null)
    {
        if (FindObjectExists(entityId, ObjectType.Entity) == EntityState.Ok)
        {
            _logger.LogWarning($"Entity {entityId} already exists");
            return EntityState.AlreadyExists;
        }

        Entity addEntity = entityType switch
        {
            EntityType.Project => Project.CreateProject(_logger, entityId, description),
            EntityType.Task => Task.CreateTask(_logger, entityId, description)
        };

        if (addEntity == null)
        {
            _logger.LogError($"Failed to create entity {entityId}");
            return EntityState.Forbidden;
        }

        _entities.Add(entityId, addEntity);
        AppendMessageToReport($"Entity {entityId} added");
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
                if (value is not EntityStatus status)
                {
                    _logger.LogError($"Cannot update entity property {property} because value is not a EntityStatus");
                    return EntityState.Forbidden;
                }

                if (status == _entities[entityId].GetStatus())
                {
                    AppendMessageToReport($"Entity's {entityId}, {property.ToString()} is already {status.ToString()}");
                    return EntityState.AlreadyExists;
                }

                switch (status)
                {
                    case EntityStatus.InProgress:
                        _entities[entityId].SetStartedDate(DateTime.UtcNow);
                        break;
                    case EntityStatus.Done:
                        _entities[entityId].SetFinishedDate(DateTime.UtcNow);
                        break;
                }

                _entities[entityId].SetStatus(status);
                AppendMessageToReport($"Entity's {entityId}, {property} updated");
                return EntityState.Ok;

            case EntityProperty.EntityPriority:
                if (value is not EntityPriority entityPriority)
                {
                    _logger.LogError($"Cannot update entity property {property} because value is not a EntityPriority");
                    return EntityState.Forbidden;
                }

                _entities[entityId].SetPriority(entityPriority);
                AppendMessageToReport($"Entity's {entityId}, {property} updated");
                return EntityState.Ok;

            default:
                _logger.LogError($"{property} not settable");
                return EntityState.Forbidden;
        }
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
                if (value is not string s)
                {
                    _logger.LogError(
                        $"Cannot update user property {property.ToString()} because value is not a string");
                    return EntityState.Forbidden;
                }

                if (string.IsNullOrEmpty(s))
                {
                    _logger.LogError($"Cannot update user property {property.ToString()} because user id is empty");
                    return EntityState.Forbidden;
                }

                _users.Remove(userId);
                _users.Add(s, user);
                _logger.LogInformation($"User {userId} updated to {s}");
                return EntityState.Ok;
            case UserProperty.AddPermissions or UserProperty.RemovePermissions:
            default:
                if (value is not Permission permission)
                {
                    _logger.LogError(
                        $"Cannot update user property {property.ToString()} because value is not a Permission");
                    return EntityState.Forbidden;
                }

                var removeUserPermission = property == UserProperty.AddPermissions
                    ? user.UserPermissions.Add(permission)
                    : user.UserPermissions.Remove(permission);
                return removeUserPermission ? EntityState.Ok : EntityState.Forbidden;
        }
    }

    /// <summary>
    /// Returns a given user report
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public object GetUserReport(string userId)
    {
        if (!_users.TryGetValue(userId, out var user))
        {
            _logger.LogError($"User {userId} does not exist");
            return EntityState.NotFound;
        }

        return user.UserReport;
    }

    /// <summary>
    /// Returns a summary of user associations
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public object GetUserAssociations(string userId)
    {
        if (!_userToEntityAssociation.TryGetValue(userId, out var value))
        {
            _logger.LogError($"User {userId} does not exist");
            return EntityState.NotFound;
        }

        if (value.Count == 0)
        {
            _logger.LogError($"User {userId} does not have associations");
            return EntityState.NotFound;
        }

        string associationDetails = string.Empty;
        foreach (var entityId in value)
        {
            associationDetails += $"[{userId} is associated with {entityId}\n";
        }

        return associationDetails;
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

        if (_userToEntityAssociation.TryAdd(userId, new HashSet<string> { entityId }))
        {
            AppendMessageToReport($"New User {userId} is associated to {entityId}");
            return AssociationStatus.NoError;
        }

        if (_userToEntityAssociation[userId].Contains(entityId))
        {
            _logger.LogDebug($"User {userId} is already associated to entity {entityId}");
            return AssociationStatus.DuplicatedAssociation;
        }

        _userToEntityAssociation[userId].Add(entityId);
        AppendMessageToReport($"User {userId} add associated to entity {entityId}");
        return AssociationStatus.NoError;
    }

    /// <summary>
    /// Disassociates User to Entity
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public AssociationStatus DisassociateUserFromEntity(string entityId, string userId)
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

        if (!_userToEntityAssociation.TryGetValue(userId, out var set) || !set.Contains(entityId))
        {
            _logger.LogError($"User {userId} has no entity association or is not associated to entity {entityId}");
            return AssociationStatus.NoAssociation;
        }

        _userToEntityAssociation[userId].Remove(entityId);
        AppendMessageToReport($"User {userId} remove associated from entity {entityId}");
        return AssociationStatus.NoError;
    }

    /// <summary>
    /// Internally associates project to task
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="taskId"></param>
    /// <returns></returns>
    private AssociationStatus AssociateEntities(string projectId, string taskId)
    {
        if (_entityToEntityAssociation.TryAdd(projectId, new HashSet<string> { taskId }))
        {
            _logger.LogWarning($"New project {projectId} is associated to task {taskId}");
            return AssociationStatus.NoError;
        }

        if (_entityToEntityAssociation.TryGetValue(projectId, out var set) && set.Contains(taskId))
        {
            _logger.LogWarning($"Project {projectId} already associated to task {taskId}");
            return AssociationStatus.DuplicatedAssociation;
        }

        _entityToEntityAssociation[projectId].Add(taskId);
        AppendMessageToReport($"Project {projectId} associated to task {taskId}");
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

        if (_entities[entityA].Type == EntityType.Project && _entities[entityB].Type == EntityType.Task)
        {
            return AssociateEntities(entityA, entityB);
        }

        if (_entities[entityA].Type == EntityType.Task && _entities[entityB].Type == EntityType.Project)
        {
            return AssociateEntities(entityB, entityA);
        }

        _logger.LogError($"Cannot associate entity {entityA} to {entityB}");
        return AssociationStatus.InvalidAssociation;
    }

    /// <summary>
    /// Internally disassociates project to task
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="taskId"></param>
    /// <returns></returns>
    private AssociationStatus DisassociateEntities(string projectId, string taskId)
    {
        if (!_entityToEntityAssociation.TryGetValue(projectId, out var set) || !set.Contains(taskId))
        {
            _logger.LogError($"Project {projectId} has no associations or is not associated to task {taskId}");
            return AssociationStatus.NoAssociation;
        }

        _entityToEntityAssociation[projectId].Remove(taskId);
        AppendMessageToReport($"Project {projectId} removed association to task {taskId}");
        return AssociationStatus.NoError;
    }

    /// <summary>
    /// Disassociates Entity from another Entity
    /// </summary>
    /// <param name="entityA"></param>
    /// <param name="entityB"></param>
    /// <returns></returns>
    public AssociationStatus DisassociateEntityFromEntity(string entityA, string entityB)
    {
        if (FindObjectExists(entityA, ObjectType.Entity) != EntityState.Ok ||
            FindObjectExists(entityB, ObjectType.Entity) != EntityState.Ok)
        {
            _logger.LogError("One or both entities not found");
            return AssociationStatus.EntityNotFound;
        }

        if (_entities![entityA].Type == EntityType.Project && _entities[entityB].Type is EntityType.Task)
        {
            return DisassociateEntities(entityA, entityB);
        }

        if (_entities[entityA] is Task && _entities[entityB] is Project)
        {
            return DisassociateEntities(entityB, entityA);
        }

        _logger.LogError($"Cannot disassociate entity {entityA} to {entityB}");
        return AssociationStatus.InvalidAssociation;
    }

    /// <summary>
    /// Returns logged user
    /// </summary>
    /// <returns></returns>
    public IUser GetCurrentUser() => _currentUser;

    private void SetPersistencePath()
    {
        PersistencePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PMSCore");
        if (!Directory.Exists(PersistencePath))
        {
            Directory.CreateDirectory(PersistencePath);
        }
    }

    private T? DeserializeObjects<T>(string path, T? objectToDeserialize)
    {
        if (!File.Exists(path))
        {
            _logger.LogError($"File {path} does not exist");
            return default;
        }

        var json = File.ReadAllText(path);
        if (string.IsNullOrEmpty(json))
        {
            _logger.LogError($"File {path} dis empty");
            return default;
        }

        objectToDeserialize = JsonSerializer.Deserialize<T>(json);
        if (objectToDeserialize == null)
        {
            _logger.LogError($"{path} file not found or failed to load from persistence");
            return default;
        }

        return objectToDeserialize;
    }

    /// <summary>
    /// Loads json data and deserializes it back to data
    /// </summary>
    private void LoadFromPersistence()
    {
        _entities = DeserializeObjects(Path.Combine(PersistencePath, "entities.json"), _entities) ?? new();
        _users = DeserializeObjects(Path.Combine(PersistencePath, "users.json"), _users) ?? new();
        _entityToEntityAssociation =
            DeserializeObjects(Path.Combine(PersistencePath, "entityToEntity.json"), _entityToEntityAssociation) ??
            new();
        _userToEntityAssociation =
            DeserializeObjects(Path.Combine(PersistencePath, "userToEntity.json"), _userToEntityAssociation) ?? new();
        FileRead = true;
    }

    /// <summary>
    /// Async method stores all data into json
    /// </summary>
    /// <returns></returns>
    public async Task<bool> StoreToPersistence()
    {
        var options = new JsonSerializerOptions();
        options.WriteIndented = true;
        await File.WriteAllTextAsync(Path.Combine(PersistencePath, "entities.json"),
            JsonSerializer.Serialize(_entities, options));
        await File.WriteAllTextAsync(Path.Combine(PersistencePath, "users.json"),
            JsonSerializer.Serialize(_users, options));
        await File.WriteAllTextAsync(Path.Combine(PersistencePath, "entityToEntity.json"),
            JsonSerializer.Serialize(_entityToEntityAssociation, options));
        await File.WriteAllTextAsync(Path.Combine(PersistencePath, "userToEntity.json"),
            JsonSerializer.Serialize(_userToEntityAssociation, options));
        return true;
    }

    private static StateManager? _singleInstance;
    private static ILogger _logger;
    private Dictionary<string, User>? _users;
    private Dictionary<string, Entity>? _entities;
    private Dictionary<string, HashSet<string>>? _userToEntityAssociation;
    private Dictionary<string, HashSet<string>>? _entityToEntityAssociation;
    private IUser? _currentUser;
    private string _report;
    private static readonly object _createLock = new();
    private string PersistencePath { get; set; }
    public bool FileRead { get; private set; } = true;
}