namespace PMSCore.Test;

/// <summary>
/// Test User Mock class that implements User interface for dependency injection
/// </summary>
internal class UserFake : User
{
    public UserFake(string userId, HashSet<Permission> userPermissions) : base(userId, userPermissions)
    {
    }
}