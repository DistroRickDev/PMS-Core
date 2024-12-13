namespace PMSCore
{
    /// <summary>
    /// An enum representing a user's access permission levels.
    /// </summary>
    public enum Permission
    {
        DEFAULT = 0,
        USER = 1,
        EMPLOYEE = 2,
        MANAGER = 3,
        ADMIN = 4,
        SUPERUSER = 5,
        TESTER = 6,
        DEVELOPER = 7,
        PROJECT_OWNER = 8
    }
}