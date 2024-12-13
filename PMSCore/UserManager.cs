using Microsoft.Extensions.Logging;
using PMSCore;

namespace PMSCore
{
    public class UserManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class.
        /// </summary>
        /// <param name="logger">Logger instance for logging operations.</param>
        private UserManager(ILogger? logger)
        {
            // Use the provided logger or create a new one if null.
            _logger = logger ?? new LoggerFactory().CreateLogger("UserManager");

            // Initialize new List to _RegisteredUsers.
            this._RegisteredUsers = new List<IUser>();

        }

        /// <summary>
        /// Retrieves the singleton instance of <see cref="UserManager"/>.
        /// Creates a new instance if it does not already exist.
        /// </summary>
        /// <param name="logger">Logger instance for logging operations.</param>
        /// <returns>The singleton instance of <see cref="UserManager"/>.</returns>
        public static UserManager GetInstance(ILogger? logger = null)
        {
            if (_singleInstance == null)
            {
                _singleInstance = new UserManager(logger);
            }

            return _singleInstance;
        }

        /// <summary>
        /// Resets the singleton instance of <see cref="UserManager"/>.
        /// </summary>
        public static void ResetInstance()
        {
            _singleInstance = null;
        }

        /// <summary>
        /// Creates a new user, assigns permissions, and attempts to store it within the entity handler.
        /// </summary>
        /// <param name="userID">The user to be created.</param>
        /// <param name="permission">The permission level to assign. Defaults to <see cref="Permission.DEFAULT"/>.</param>
        /// <returns><c>true</c> if the user was created successfully; otherwise, <c>false</c>.</returns>
        public bool _CreateUser(IUser userID, Permission permission = Permission.DEFAULT)
        {
            // TODO: Class StateManager to insert User to persistent file system and return confirmation token.
            return true;
        }

        /// <summary>
        /// Changes the permission level of a specified user.
        /// </summary>
        /// <param name="user">The user whose permissions are being changed.</param>
        /// <param name="permission">The new permission level to assign.</param>
        /// <returns><c>true</c> if the permissions were changed successfully; otherwise, <c>false</c>.</returns>
        public bool ChangeUserPermission(IUser currentUser, IUser targetUser, Permission permission)
        {
            // TODO: Class StateHandler to manage a User's permissions access.
            // StateHandler should validate _currentUser and check for permission level before applying.

            bool result = false;

            if (!(targetUser == null) &&  !(currentUser == null) && (currentUser.GetPermission() >= Permission.ADMIN || currentUser.Equals(targetUser)))
            {

                    targetUser.SetPermission(permission);
                    result = true;
            }
            else
            {
                _logger.LogError("UserID cannot be null or empty.");
            }
            return result;
        }


        /// <summary>
        /// Retrieves the permission level of the specified user.
        /// </summary>
        /// <param name="user">The user whose permission level is being retrieved.</param>
        /// <returns>The permission level of the user as <see cref="Permission"/>.</returns>
        public Permission GetUserPermissionLevel(IUser user)
        {
            return user.GetPermission();
        }


        /// <summary>
        /// Retrieves the user ID of the specified user.
        /// </summary>
        /// <param name="user">The user whose ID is being retrieved.</param>
        /// <returns>The user ID as a string.</returns>
        public string GetUserID(IUser user)
        {
            return user.GetUserID();
        }

        public bool AddUser(IUser user)
        {
            if(!(user == null))
            {
                this._RegisteredUsers.Add(user);
                return true;
            }
            return false;
        }


        /// <summary>
        /// List holding the system's registered users.
        /// </summary>
        private List<IUser> _RegisteredUsers;

        /// <summary>
        /// Static instance of UserManager, enforcing the Singleton pattern.
        /// </summary>
        private static UserManager? _singleInstance;

        /// <summary>
        /// Logger for the UserManager class.
        /// </summary>
        private readonly ILogger _logger;
    }
}
