namespace PMSCore
{
    /// <summary>
    /// Represents a user's access permission levels.
    /// </summary>
    internal class Permission_Object
    {
        /// <summary>
        /// Enum defining different user access levels.
        /// </summary>
        private enum Permission_Levels
        {
            DEFAULT = 0,    // Default permission level.
            EMPLOYEE = 1,   // Employee-level permissions.
            TESTER = 2,     // Tester-level permissions.
            MANAGER = 3,    // Manager-level permissions.
            ADMIN = 4,      // Admin-level permissions.
            SUPERUSER = 5   // Superuser-level permissions.
        }

        private readonly ILogger _logger; // Logger instance for the Permission_Object class.
        private Permission_Levels _permissionLevel; // Stores the user's permission level.

        /// <summary>
        /// Constructor for Permission_Object. Initializes the permission level.
        /// </summary>
        /// <param name="logger">Optional logger instance for logging operations.</param>
        /// <param name="permissionLevel">Integer representing the user's permission level.</param>
        public Permission_Object(ILogger? logger, int permissionLevel)
        {
            _logger = logger ?? new LoggerFactory().CreateLogger("Permission_Object"); // Use provided logger or create a default logger.

            try
            {
                // Validate the provided permission level against the enum values.
                if (Enum.IsDefined(typeof(Permission_Levels), permissionLevel))
                {
                    _permissionLevel = (Permission_Levels)permissionLevel; // Assign the valid permission level.
                    _logger.LogInformation("PERMISSION CREATED: {PermissionLevel}", GetPermissionLevel());
                }
                else
                {
                    // TODO: Replace this generic exception with a custom exception for invalid permissions.
                    throw new Exception("Invalid permission level.");
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                // Log a critical error when an invalid permission level is encountered.
                _logger.LogCritical("INVALID PERMISSION OBJECT CREATION ATTEMPT\nATTEMPTED INPUT: {PermissionLevel}", permissionLevel);
            }
        }

        /// <summary>
        /// Retrieves the permission level as a string.
        /// </summary>
        /// <returns>A string representation of the user's permission level.</returns>
        public string GetPermissionLevel()
        {
            // Log the access of the permission level.
            _logger.LogInformation("Permission Accessed: {ThisPermission}", _permissionLevel.ToString());
            return _permissionLevel.ToString();
        }
    }
}
