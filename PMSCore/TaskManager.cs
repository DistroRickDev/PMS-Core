using System.Reflection;

namespace PMSCore;

public class TaskManager
{
    private TaskManager(ILogger? logger)
    {
        _logger = logger ?? new LoggerFactory().CreateLogger("ProjectManager");
    }

    /// <summary>
    /// Returns singleton instance for ProjectManager, creates a new instance if current on is null
    /// </summary>
    /// <returns></returns>
    public static TaskManager GetInstance(ILogger? logger = null)
    {
        if (_singleInstance == null)
        {
            _singleInstance = new TaskManager(logger);
        }

        return _singleInstance;
    }
    
    /// <summary>
    /// Resets singleton instance
    /// </summary>
    public static void ResetInstance()
    {
        _singleInstance = null;
    }


    /// <summary>
    /// Creates a new task and attempts to store within the Entity handler
    /// </summary>
    /// <param name="taskName"></param>
    /// <param name="description"></param>
    /// <param name="priority"></param>
    /// <returns>TaskState</returns>
    public bool CreateTask(string taskName, string description = "", TaskPriority priority = TaskPriority.Medium)
    {
        var task = new Task(taskName, description, priority);
        // TODO: Class StateHandler to insert Task to Entity (Project|User) and return TaskState (Created | Error) 
        return true;
    }

    /// <summary>
    /// Generic Association Manager method it associates a given Task to (IUser|Project)
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="entity"></param>
    /// <param name="disassociate"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private AssociationStatus ManageAssociateToTask(string? taskId, string? entity, AssociationType disassociate, Type type)
    {
        if (!string.IsNullOrEmpty(entity) && !string.IsNullOrEmpty(taskId))
        {
            switch (disassociate)
            {
                case AssociationType.Associate:
                    // TODO: Class StateHandler to insert Task to Entity (Project|User) dictionary 
                    return AssociationStatus.NoError;
                case AssociationType.Disassociate:
                    // TODO: Class StateHandler to remove Task to Entity (Project|User) dictionary 
                    return AssociationStatus.NoError;
            }
        }

        if (!string.IsNullOrEmpty(taskId))
        {
            _logger.LogError("TaskID cannot be null. Make sure TaskID is valid for trying to get association.");
            return AssociationStatus.InvalidTask;
        }
    
        _logger.LogError($"{type}ID cannot be null. Make sure {type}ID is valid for trying to get association.");
        return type == typeof(Project) ? AssociationStatus.InvalidProject : AssociationStatus.InvalidUser;
    }

    /// <summary>
    /// Method that associates a given task to a user
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public AssociationStatus AssociateTaskToUser(string? taskId, string? userId)
    {
        return ManageAssociateToTask(taskId, userId, AssociationType.Associate, typeof(IUser));
    }

    /// <summary>
    /// Method that associates a given task to a project
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="projectId"></param>
    /// <returns></returns>
    public AssociationStatus AssociateTaskToProject(string? taskId, string? projectId)
    {
        return ManageAssociateToTask(taskId, projectId, AssociationType.Associate, typeof(Project));
    }

    /// <summary>
    /// Method that removes association between a given task to a user
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public AssociationStatus RemoveTaskFromUser(string? taskId, string? userId)
    {
        return ManageAssociateToTask(taskId, userId, AssociationType.Disassociate, typeof(IUser));
    }

    /// <summary>
    ///  Method that removes association between a given task to a project
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="projectId"></param>
    /// <returns></returns>
    public AssociationStatus RemoveTaskFromProject(string? taskId, string? projectId)
    {
        return ManageAssociateToTask(taskId, projectId, AssociationType.Disassociate, typeof(Project));
    }

    /// <summary>
    /// Generic method that manages to call StateHandler to change a Task property
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="propertyValue"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private bool ChangeTaskProperty<T>(string? taskId, T propertyValue)
    {
        // TODO: Class StateHandler to change a Task property, StateHandler should validate currentUser and check for permission
        if (!string.IsNullOrEmpty(taskId))
        {
            return true;
        }
        _logger.LogError("TaskID cannot be null or empty.");
        return false;  
    }

    /// <summary>
    /// Generic method that manages to call StateHandler to change a Task name or description
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="propertyValue"></param>
    /// <param name="isName"></param>
    /// <returns></returns>
    private bool ChangeTaskProperty(string? taskId, string? propertyValue, bool isName)
    {
        // TODO: Class StateHandler to change a Task property, StateHandler should validate currentUser and check for permission
        if (!string.IsNullOrEmpty(taskId))
        {
            return true;
        }

        if (isName && string.IsNullOrEmpty(propertyValue))
        {   
            _logger.LogError("Task name cannot be null or empty.");
            return false;
        }
        _logger.LogError("TaskID cannot be null or empty.");
        return false;  
    }

    /// <summary>
    ///  Method that changes an existing Task status
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="newStatus"></param>
    /// <returns></returns>
    public bool ChangeTaskStatus(string? taskId, TaskStatus newStatus)
    {
        return ChangeTaskProperty(taskId, newStatus);
    }
    
    /// <summary>
    ///  Method that changes an existing Task name
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="newName"></param>
    /// <returns></returns>
    public bool ChangeTaskName(string? taskId, string newName)
    {
        return ChangeTaskProperty(taskId, newName, true);
    }
    
    /// <summary>
    ///  Method that changes an existing Task name
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="newDescription"></param>
    /// <returns></returns>
    public bool ChangeTaskDescription(string? taskId, string newDescription)
    {
        return ChangeTaskProperty(taskId, newDescription, false);
    }
    
    private static TaskManager? _singleInstance;
    private readonly ILogger _logger;
}
