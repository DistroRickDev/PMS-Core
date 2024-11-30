namespace PMSCore;

using Microsoft.Extensions.Logging;

/// <summary>
/// Enum representing possible errors on association between User and Project
/// </summary>
public enum AssociationStatus
{
    NoError = 0,
    InvalidUser,
    InvalidProject,
    DuplicatedProjectToUserAssociation,
    UserNotFound,
    ProjectNotFound,
}

/// <summary>
/// Associative class that manages project per user and project lifeCycle
/// </summary>
public class ProjectManager
{
    private ProjectManager()
    {
        _logger = new LoggerFactory().CreateLogger("ProjectManager");
    }

    /// <summary>
    /// Returns singleton instance for ProjectManager, creates a new instance if current on is null
    /// </summary>
    /// <returns></returns>
    public static ProjectManager GetInstance()
    {
        if (_singleInstance == null)
        {
            _singleInstance = new ProjectManager();
        }

        return _singleInstance;
    }

    /// <summary>
    /// Validates if project or user is invalid and returns error code
    /// </summary>
    /// <param name="project"></param>
    /// <param name="user"></param>
    /// <returns>AssociationStatus</returns>
    private AssociationStatus _validateAssociation(Project project, IUser user)
    {
        if (project == null)
        {
            _logger.LogError("Project cannot be null.");
            return AssociationStatus.InvalidProject;
        }

        if (user == null)
        {
            _logger.LogError("User cannot be null.");
            return AssociationStatus.InvalidUser;
        }

        return AssociationStatus.NoError;
    }

    /// <summary>
    /// Static method responsible to associate a give Project to a given User
    /// </summary>
    /// <param name="project"></param>
    /// <param name="user"></param>
    /// <returns>AssociationStatus</returns>
    public AssociationStatus AssociateProjectToUser(Project project, IUser user)
    {
        var associationStatus = _validateAssociation(project, user);
        if (associationStatus != AssociationStatus.NoError)
        {
            return associationStatus;
        }

        if (_projectToUserMap.ContainsKey(user))
        {
            if (_projectToUserMap[user].Contains(project))
            {
                _logger.LogWarning("User {user}, already contains {project}", user, project);
                return AssociationStatus.DuplicatedProjectToUserAssociation;
            }

            _logger.LogDebug("User {user}, already exists, appending project  {project}", user, project);
            _projectToUserMap[user].Append(project);
            return AssociationStatus.NoError;
        }

        _logger.LogInformation("Adding new user {user}, appending project {project}", user, project);
        _projectToUserMap.Add(user, new List<Project> { project });
        return AssociationStatus.NoError;
    }

    /// <summary>
    /// Removes project from User
    /// </summary>
    /// <param name="project"></param>
    /// <param name="user"></param>
    /// <returns>AssociationStatus</returns>
    public AssociationStatus RemoveProjectFromUser(Project project, IUser user)
    {
        var associationStatus = _validateAssociation(project, user);
        if (associationStatus != AssociationStatus.NoError)
        {
            return associationStatus;
        }

        if (!_projectToUserMap.ContainsKey(user))
        {
            _logger.LogError("User {user}, not found", user);
            return AssociationStatus.UserNotFound;
        }

        if (!_projectToUserMap[user].Contains(project))
        {
            _logger.LogError("User {user}, do not contains {project}", user, project);
            return AssociationStatus.ProjectNotFound;
        }

        _logger.LogInformation("Removing project {project} from {user}", project, user);
        _projectToUserMap[user].Remove(project);
        return AssociationStatus.NoError;
    }

    private static ProjectManager _singleInstance;
    private static Dictionary<IUser, List<Project>> _projectToUserMap { get; } = new();
    private static ILogger _logger;
}