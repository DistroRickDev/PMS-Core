using Microsoft.Extensions.Logging;
using System.Security;

namespace PMSCore.Test;

/// <summary>
/// Test User Mock class that implements User interface for dependency injection
/// </summary>
internal class UserFake : IUser
{
    private Permission _UserPermission; // Stores the permission object for the user.
    private readonly string _userID; // Stores the user ID.
    private readonly ILogger _logger; // Logger for the User class.
                                      //private readonly List<Project> _ProjectList; // List of user's projects.

    /// <summary>
    /// Constructor for the User class. Initializes permissions and logs user details.
    /// </summary>
    /// <param name="logger">Logger instance.</param>
    /// <param name="permission">Permission object for the user.</param>
    public UserFake(string userID, Permission permission = Permission.DEFAULT, ILogger? logger = null)
    {
        this._logger = logger ?? new LoggerFactory().CreateLogger("UserFake");

        if (!(string.IsNullOrEmpty(permission.ToString())))
        {
            this._UserPermission = permission; // Assigns permission object.
            _logger.LogDebug("Assigning User: {userID} Permissions:\n{userPermission}.", userID, this._UserPermission);
        }
        else
        {
            _logger.LogError($"Invalid Permission{permission}");
            return;
        }

        if (!(string.IsNullOrEmpty(userID)))
        {
            this._userID = userID;
            _logger.LogDebug("ASSIGNED USERID: {userID}\nPERMISSION LEVEL: {userPermission}", this._userID, this._UserPermission);
        }
        else
        {
            Console.WriteLine($"Invalid userID. UserID cannot be null or empty.");
            _logger.LogError($"UserID Error: {userID}");
            return;
        }
    }
    public string GetUserID()
    {
        return this._userID;
    }

    
    // TODO: Figure out if we still need this here and in IUser?
    public bool Login(string userName)
    {
        throw new NotImplementedException();
    }

    // TODO: Figure out if we still need this here and in IUser?
    public bool Register(string userName)
    {
        throw new NotImplementedException();
    }

    public Permission GetPermission()
    {
        return this._UserPermission;
    }

    public void SetPermission(Permission permission)
    {
        this._UserPermission = permission;
    }

}