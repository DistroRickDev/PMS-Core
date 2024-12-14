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

        public IUser SearchUsers(string targetUserID)
        {
            foreach (IUser user in _RegisteredUsers)
            {
                if (targetUserID.Equals(user.GetUserID()))
                {
                    return user;
                }
            }
            return null;
        }

        /// <summary>
        /// Changes the permission level of a specified user.
        /// </summary>
        /// <param name="targetUserID">The ID of the target user whose permissions are being changed.</param>
        /// <param name="permission">The new permission level to assign.</param>
        /// <returns><c>true</c> if the permissions were changed successfully; otherwise, <c>false</c>.</returns>
        public AssociationStatus ChangeUserPermission(string targetUserID, Permission permission) {

            if ((targetUserID == null) || (_currentUser == null))
            {
                _logger.LogError("UserID cannot be null or empty.");
                return AssociationStatus.InvalidUser;
            }

            IUser targetUser = SearchUsers(targetUserID);

            if (targetUser != null)
            {
                if (_currentUser.GetPermission() >= Permission.ADMIN || _currentUser.Equals(targetUser))
                {
                    targetUser.SetPermission(permission);
                    return AssociationStatus.NoError;
                }
                else
                {
                    return AssociationStatus.InvalidPermission;
                }

            }

            return AssociationStatus.UserNotFound;
       }

        /// <summary>
        /// Logs in the specified user if they are registered.
        /// </summary>
        /// <param name="loginUser">The user attempting to log in.</param>
        /// <returns><see cref="UserStatus.OK"/> if login is successful; otherwise if _RegisteredUser's does not contain user returns, <see cref="UserStatus.NotFound"/>. Else returns <see cref="UserStatus.Error"/></returns>
        public UserStatus Login(IUser loginUser)
        {
            if (_currentUser == null)
            {
                if (_RegisteredUsers.Contains(loginUser))
                {
                    _currentUser = loginUser;
                    return UserStatus.OK;
                }

                else
                {
                    return UserStatus.NotFound;
                }
            }
            return UserStatus.Error;
        }

        /// <summary>
        /// Registers a new user to the system if they are not already registered.
        /// </summary>
        /// <param name="newUser">The user to be registered.</param>
        /// <returns><see cref="UserStatus.OK"/> if registration is successful; otherwise, <see cref="UserStatus.Found"/>.</returns>
        public UserStatus Register(IUser newUser)
        {
            if (!(_RegisteredUsers.Contains(newUser)))
            {
                _RegisteredUsers.Add(newUser);
                return UserStatus.OK;
            }
            return UserStatus.Found;
        }

        /// <summary>
        /// Deletes a specific user from the _RegisteredUsers list. 
        /// </summary>
        /// <param name="targetUser">The user selected for deletion</param>
        /// <returns><see cref="UserStatus.OK"/> if user deletion successful; otherwise, <see cref="UserStatus.NotFound"/> if the user does not exists.</returns>
        public UserStatus DeleteUser(IUser targetUser)
        {
            // TODO: Update to tie in with StateManager.
            if (_RegisteredUsers.Contains(targetUser))
            {
                _RegisteredUsers.Remove(targetUser);
                return UserStatus.OK;
            }

            return UserStatus.NotFound;
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
        /// Current user logged into the system.
        /// </summary>
        private IUser _currentUser;

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
