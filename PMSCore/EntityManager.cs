using System.Reflection;

namespace PMSCore;

/// <summary>
/// Class that manages project entities and communicates with StateManager to manage the entity states
/// </summary>
public class EntityManager
{
    private EntityManager(ILogger? logger)
    {
        _logger = logger ?? new LoggerFactory().CreateLogger("TaskManager");
    }

    /// <summary>
    /// Returns singleton instance for TaskManager, creates a new instance if current on is null
    /// </summary>
    /// <returns></returns>
    public static EntityManager GetInstance(ILogger? logger = null)
    {
        lock (_createLock)
        {
            if (_singleInstance == null)
            {
                _singleInstance = new EntityManager(logger);
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
    /// Creates a new entity and attempts to store within the Entity handler
    /// </summary>
    /// <param name="id"></param>
    /// <param name="description"></param>
    /// /// <param name="entityType"></param>
    /// <returns>EntityState</returns>
    public EntityState CreateEntity(string id, string? description, EntityType entityType)
    {
        if (entityType == EntityType.Project)
        {
            return StateManager.GetInstance().CreateEntity(Project.CreateProject(_logger, id, description)!);
        }
        return StateManager.GetInstance().CreateEntity(Task.CreateTask(_logger, id, description)!);
    }
    
    /// <summary>
    /// Removes a given entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EntityState RemoveEntity(string id)
    {
        return StateManager.GetInstance().RemoveEntity(id);
    }


    /// <summary>
    /// Updates Entity description
    /// </summary>
    /// <param name="id"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public EntityState UpdateEntityDescription(string id, string? value)
    {
        return StateManager.GetInstance().UpdateEntityProperty(id, EntityProperty.Description, value);
    }

    /// <summary>
    /// Returns Entity Description
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public (EntityState, string?) GetEntityDescription(string id)
    {
        var res = StateManager.GetInstance().GetEntityProperty(id, EntityProperty.Description);
        return (res.Item1, (string?)res.Item2);
    }

    /// <summary>
    /// Updates Entity status
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public EntityState UpdateEntityStatus(string id, EntityStatus status)
    {
        return StateManager.GetInstance().UpdateEntityProperty(id, EntityProperty.EntityStatus, status);
    }

    /// <summary>
    /// Gets Entity status
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public (EntityState, EntityStatus?) GetEntityStatus(string id)
    {
        var res = StateManager.GetInstance().GetEntityProperty(id, EntityProperty.EntityStatus);
        return (res.Item1, (EntityStatus)res.Item2!);
    }

    /// <summary>
    /// Updates Entity status
    /// </summary>
    /// <param name="id"></param>
    /// <param name="priority"></param>
    /// <returns></returns>
    public EntityState UpdateEntityPriority(string id, EntityPriority priority)
    {
        return StateManager.GetInstance().UpdateEntityProperty(id, EntityProperty.EntityPriority, priority);
    }

    /// <summary>
    /// Gets Entity status
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public (EntityState, EntityPriority?) GetEntityPriority(string id)
    {
        var res = StateManager.GetInstance().GetEntityProperty(id, EntityProperty.EntityPriority);
        return (res.Item1, (EntityPriority?)res.Item2!);
    }

    /// <summary>
    /// Sets Entity start date
    /// </summary>
    /// <param name="id"></param>
    /// <param name="startDate"></param>
    /// <returns></returns>
    public EntityState UpdateEntityStartDate(string id, DateTime startDate)
    {
        return StateManager.GetInstance().UpdateEntityProperty(id, EntityProperty.StartedDate, startDate);
    }

    /// <summary>
    /// Gets Entity start date
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public (EntityState, DateTime?) GetEntityStartDate(string id)
    {
        var res = StateManager.GetInstance().GetEntityProperty(id, EntityProperty.StartedDate);
        return (res.Item1, (DateTime?)res.Item2!);
    }

    /// <summary>
    /// Sets Entity finish date
    /// </summary>
    /// <param name="id"></param>
    /// <param name="finishDate"></param>
    /// <returns></returns>
    public EntityState UpdateEntityFinishDate(string id, DateTime finishDate)
    {
        return StateManager.GetInstance().UpdateEntityProperty(id, EntityProperty.FinishedDate, finishDate);
    }

    /// <summary>
    /// Gets Entity finish date
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public (EntityState, DateTime?) GetEntityFinishDate(string id)
    {
        var res = StateManager.GetInstance().GetEntityProperty(id, EntityProperty.FinishedDate);
        return (res.Item1, (DateTime?)res.Item2!);
    }

    /// <summary>
    /// Gets Entity creation date
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public (EntityState, DateTime) GetEntityCreationDate(string id)
    {
        var res = StateManager.GetInstance().GetEntityProperty(id, EntityProperty.CreatedDate);
        return (res.Item1, (DateTime)res.Item2!);
    }

    /// <summary>
    /// Gets Entity report
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public (EntityState, string?) GetEntityReport(string id)
    {
        var res = StateManager.GetInstance().GetEntityProperty(id, EntityProperty.Report);
        return (res.Item1, (string?)res.Item2!);
    }

    /// <summary>
    /// Associates a given Entity to a User
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public AssociationStatus AssociateEntityToUser(string entityId, string userId)
    {
        return StateManager.GetInstance().AssociateUserToEntity(entityId, userId);
    }

    /// <summary>
    /// Disassociates a given Entity to a User
    /// </summary>
    /// <param name="entityId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public AssociationStatus DisassociateEntityFromUser(string entityId, string userId)
    {
        return StateManager.GetInstance().DisassociateUserToEntity(entityId, userId);
    }

    /// <summary>
    /// Associates a given Entity to a User
    /// </summary>
    /// <param name="entityA"></param>
    /// <param name="entityB"></param>
    /// <returns></returns>
    public AssociationStatus AssociateEntityToEntity(string entityA, string entityB)
    {
        return StateManager.GetInstance().AssociateEntityToEntity(entityA, entityB);
    }

    /// <summary>
    /// Disassociates a given Entity to a User
    /// </summary>
    /// <param name="entityA"></param>
    /// <param name="entityB"></param>
    /// <returns></returns>
    public AssociationStatus DisassociateEntityFromEntity(string entityA, string entityB)
    {
        return StateManager.GetInstance().DisassociateUserToEntity(entityA, entityB);
    }

    private static EntityManager? _singleInstance;
    private readonly ILogger _logger;
    private static readonly object _createLock = new();
}