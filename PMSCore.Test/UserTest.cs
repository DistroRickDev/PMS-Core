namespace PMSCore.Test;

public class UserTest
{
    public UserTest()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PMSCore");
        var destinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PMSCore-PROD");
        PreserveProductionPersistence(path, destinationPath);
    }

    ~UserTest()
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PMSCore-PROD");
        var destinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "PMSCore");
        PreserveProductionPersistence(path, destinationPath);
    }

    private static void PreserveProductionPersistence(string path, string destinationPath)
    {
        if (!Directory.Exists(path)) return;
        if (Directory.Exists(destinationPath)) return;
        Directory.Move(path, destinationPath);
    }

    /// <summary>
    /// Helper function to clean up singleton on start of every test
    /// </summary>
    private void SetUp()
    {
        StateManager.ResetInstance();
        Thread.Sleep(150);
    }

    /// <summary>
    /// Tests user abstract functionality happy path
    /// </summary>
    [Fact]
    public void UserTest_HappyPath()
    {
        SetUp();
        const string userId = "Admin42";
        const string taskId = "taskId";
        const string projectId = "projectId";
        const string secondUserId = "secondUserId";
        User superUser = new Admin(userId);
        Assert.Equal(superUser.UserId, userId);
        Assert.Equal(UserOperationResult.Ok, superUser.CreateEntity(EntityType.Task, taskId));
        Assert.Equal(UserOperationResult.Ok, superUser.CreateEntity(EntityType.Project, projectId));
        Assert.Equal(UserOperationResult.Ok,
            superUser.ChangeEntityProperty(EntityType.Task, taskId, EntityProperty.Description, "Description"));
        Assert.Equal(UserOperationResult.Ok,
            superUser.ChangeEntityProperty(EntityType.Project, projectId, EntityProperty.Description, "Description"));
        Assert.Equal(UserOperationResult.Ok, superUser.AssociateEntityToEntity(projectId, taskId));
        Assert.Equal(EntityState.Ok,
            StateManager.GetInstance().UserRegister(secondUserId, new Developer(secondUserId)));
        Assert.Equal(UserOperationResult.Ok, superUser.AssociateUserWithEntity(secondUserId, taskId));
        var associations = superUser.GetUserAssociations(secondUserId);
        Assert.IsType<string>(associations);
        Assert.Contains(taskId, (associations as string)!);
        Assert.Equal(UserOperationResult.Ok, superUser.ChangeUserId(secondUserId, "NewId"));
        Assert.Equal(UserOperationResult.Ok, superUser.ChangeUserId("NewId", secondUserId));
        Assert.Equal(UserOperationResult.Ok, superUser.AddUserPermission(secondUserId, Permission.CanChangeUser));
        Assert.Equal(UserOperationResult.Ok, superUser.RemoveUserPermission(secondUserId, Permission.CanChangeUser));
        Assert.Equal(UserOperationResult.Ok, superUser.DissociateEntityFromEntity(projectId, taskId));
        Assert.Equal(UserOperationResult.Ok, superUser.DisassociateUserWithEntity(secondUserId, taskId));
        Assert.IsType<string>(superUser.GenerateUserReport(secondUserId));
        Assert.IsType<string>(superUser.GenerateEntityReport(taskId));
        Assert.Equal(UserOperationResult.Ok, superUser.DeleteUser(secondUserId));
        Assert.Equal(UserOperationResult.Ok, superUser.DeleteEntity(EntityType.Task, taskId));
        Assert.Equal(UserOperationResult.Ok, superUser.DeleteEntity(EntityType.Project, projectId));
    }

    /// <summary>
    /// Tests user abstract functionality corner cases
    /// </summary>
    [Fact]
    public void UserTest_UnHappyPath()
    {
        SetUp();
        const string userId = "Admin42";
        const string taskId = "taskId";
        const string projectId = "projectId";
        const string secondUserId = "secondUserId";
        User nonSuperUser = new Tester(userId);
        Assert.Equal(nonSuperUser.UserId, userId);
        Assert.Equal(UserOperationResult.Failed, nonSuperUser.CreateEntity(EntityType.Task, taskId));
        Assert.Equal(UserOperationResult.Failed, nonSuperUser.CreateEntity(EntityType.Project, projectId));
        Assert.Equal(UserOperationResult.Failed,
            nonSuperUser.ChangeEntityProperty(EntityType.Task, taskId, EntityProperty.Description, "Description"));
        Assert.Equal(UserOperationResult.Failed,
            nonSuperUser.ChangeEntityProperty(EntityType.Project, projectId, EntityProperty.Description,
                "Description"));
        Assert.Equal(UserOperationResult.Failed, nonSuperUser.AssociateEntityToEntity(projectId, taskId));
        Assert.Equal(UserOperationResult.Failed, nonSuperUser.AssociateUserWithEntity(secondUserId, taskId));
        var associations = nonSuperUser.GetUserAssociations(secondUserId);
        Assert.IsNotType<string>(associations);
        Assert.Equal(UserOperationResult.Failed, nonSuperUser.ChangeUserId(secondUserId, "NewId"));
        Assert.Equal(UserOperationResult.Failed, nonSuperUser.ChangeUserId("NewId", secondUserId));
        Assert.Equal(UserOperationResult.Failed,
            nonSuperUser.AddUserPermission(secondUserId, Permission.CanChangeUser));
        Assert.Equal(UserOperationResult.Failed,
            nonSuperUser.RemoveUserPermission(secondUserId, Permission.CanChangeUser));
        Assert.Equal(UserOperationResult.Failed, nonSuperUser.DissociateEntityFromEntity(projectId, taskId));
        Assert.Equal(UserOperationResult.Failed, nonSuperUser.DisassociateUserWithEntity(secondUserId, taskId));
        Assert.IsNotType<string>(nonSuperUser.GenerateUserReport(secondUserId));
        Assert.IsNotType<string>(nonSuperUser.GenerateEntityReport(taskId));
        Assert.Equal(UserOperationResult.Failed, nonSuperUser.DeleteUser(secondUserId));
        Assert.Equal(UserOperationResult.Failed, nonSuperUser.DeleteEntity(EntityType.Task, taskId));
        Assert.Equal(UserOperationResult.Failed, nonSuperUser.DeleteEntity(EntityType.Project, projectId));
    }
}