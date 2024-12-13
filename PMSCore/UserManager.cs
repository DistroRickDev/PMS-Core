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
        /// Changes the permission level of a specified user.
        /// </summary>
        /// <param name="currentUser">The current system user.</param>
        /// <param name="targetUser">The user whose permissions are being changed.</param>
        /// <param name="permission">The new permission level to assign.</param>
        /// <returns><c>true</c> if the permissions were changed successfully; otherwise, <c>false</c>.</returns>
        public AssociationStatus ChangeUserPermission(IUser currentUser, IUser targetUser, Permission permission)
        {
            // TODO: Class StateHandler to manage a User's permissions access.
            // StateHandler should validate _currentUser and check for permission level before applying.

            if ((targetUser == null) || (currentUser == null))
            {
                _logger.LogError("UserID cannot be null or empty.");
                return AssociationStatus.InvalidUser;
            }
            else if (currentUser.GetPermission() >= Permission.ADMIN || currentUser.Equals(targetUser))
            {
                targetUser.SetPermission(permission);
                return AssociationStatus.NoError;
            }

            return AssociationStatus.InvalidPermission;
        }

        /// <summary>
        /// Logs in the specified user if they are registered.
        /// </summary>
        /// <param name="loginUser">The user attempting to log in.</param>
        /// <returns><see cref="AssociationStatus.NoError"/> if login is successful; otherwise, <see cref="AssociationStatus.InvalidUser"/>.</returns>
        public AssociationStatus Login(IUser loginUser)
        {
            if (_currentUser == null)
            {
                if (_RegisteredUsers.Contains(loginUser))
                {
                    _currentUser = loginUser;
                    return AssociationStatus.NoError;
                }
            }

            return AssociationStatus.InvalidUser;

        }

        /// <summary>
        /// Registers a new user to the system if they are not already registered.
        /// </summary>
        /// <param name="newUser">The user to be registered.</param>
        /// <returns><see cref="AssociationStatus.NoError"/> if registration is successful; otherwise, <see cref="AssociationStatus.InvalidUser"/>.</returns>
        public AssociationStatus Register(IUser newUser)
        {
            if (!(_RegisteredUsers.Contains(newUser)))
            {
                _RegisteredUsers.Add(newUser);
                return AssociationStatus.NoError;
            }
            return AssociationStatus.InvalidUser;
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


        public AssociationStatus AddUser(IUser user)
        {
            if (!(user == null) && !(_RegisteredUsers.Contains(user)))
            {
                this._RegisteredUsers.Add(user);
                return AssociationStatus.NoError;
            }
            return AssociationStatus.InvalidUser;
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
