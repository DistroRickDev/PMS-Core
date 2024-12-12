namespace PMSCore
{
    internal abstract class User
    {
        private Permission_Object _UserPermission; // Stores the permission object for the user.
        private readonly string _userID; // Stores the user ID.
        private readonly ILogger _logger; // Logger for the User class.

        /// <summary>
        /// Constructor for the User class. Initializes permissions and logs user details.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        /// <param name="permission">Permission object for the user.</param>
        public User(ILogger? logger, Permission_Object permission, string userID)
        {
            this._logger = logger ?? new LoggerFactory().CreateLogger("User");

            try
            {
                _logger.LogDebug("Initializing user.");
                this._userID = userID;
                this._UserPermission = permission; // Assign permission object.

                _logger.LogDebug("Assigning user permissions:\n{userPermissions}.", _UserPermission.GetPermissionLevel());
                // Display.HomeMenu(userID);
                _logger.LogDebug($"User {_userID} logged in.");
                _logger.LogDebug($"{_userID}'s PERMISSION LEVEL: {_UserPermission.GetPermissionLevel()}");

            }
            catch (Exception ex)
            {
                //TODO Create custom exceptions for failures.

                // Log critical error during user initialization.
                _logger.LogCritical("{datetime}: Invalid User.\n{errorDetails}", DateTime.Now, ex);
            }
        }

        /// <summary>
        /// Retrieves the user's permission level.
        /// </summary>
        /// <returns>A string representing the user's permission level.</returns>
        public string ViewUserPermissions()
        {
            return this._UserPermission.GetPermissionLevel();
        }

        /// <summary>
        /// Retrieves the user's ID.
        /// </summary>
        /// <returns>The user ID as a string.</returns>
        public string GetUserID()
        {
            return this._userID;
        }
    }
}

