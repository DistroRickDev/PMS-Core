namespace PMSCore.Test;

/// <summary>
/// Test User Mock class that implements User interface for dependency injection
/// </summary>
internal class UserFake : IUser
{
    public string GetUserID()
    {
        throw new NotImplementedException();
    }

    public bool Login(string userName)
    {
        throw new NotImplementedException();
    }

    public bool Register(string userName)
    {
        throw new NotImplementedException();
    }

    public void SetPermission(Permission permission)
    {
        throw new NotImplementedException();
    }

    public Permission GetPermission()
    {
        throw new NotImplementedException();
    }
}