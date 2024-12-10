namespace PMSCore
{

    /// <summary>
    /// </summary>
    public enum Permission
    {
        /// <summary>
        /// Enum representing the different levels of user access.
        /// Ranges from 0 (Default - No Access) to 5 (Superuser - Total Access).
        /// <summary>
        public enum PermissionLevel
        {
            DEFAULT = 0,
            USER = 1,
            EMPLOYEE = 2,
            MANAGER = 3,
            ADMIN = 4,
            SUPERUSER = 5
        }
}