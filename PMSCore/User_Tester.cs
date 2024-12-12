namespace PMSCore
{
    internal class User_Tester : User
    {
        private readonly ILogger _logger; // Logger for the User_Tester class.

        /// <summary>
        /// Constructor for User_Tester. Passes logger and permission object to the base class.
        /// </summary>
        /// <param name="logger">Optional logger instance.</param>
        /// <param name="permission">Permission object for the user.</param>
        internal User_Tester(ILogger? logger, Permission_Object permission, string userID) : base(logger, permission, userID)
        {
            _logger = logger ?? new LoggerFactory().CreateLogger("User_Tester");

        }
    }
}

