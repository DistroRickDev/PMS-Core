namespace PMSCore.Test;

public class UserTest
{
    private static User SuperUser { get; } = new UserFake("SuperUser", new HashSet<Permission>
    {
        Permission.CanCreateProject,
        Permission.CanChangeProjectProperty,
        Permission.CanDeleteProject,
        Permission.CanCreateTask,
        Permission.CanChangeTaskProperty,
        Permission.CanDeleteTask,
        Permission.CanGenerateEntityReport,
        Permission.CanDeleteUser,
        Permission.CanChangeUser,
        Permission.CanGenerateUserReport,
    });

    private static User UnPermittedUser { get; } = new UserFake("UnPermittedUser", new());
    
    private static string ProjectId { get; } = "TestProject";
    private static string TaskId { get; } = "TestTask";

    static void Setup()
    {
        StateManager.ResetInstance();
        Thread.Sleep(100);
        EntityManager.ResetInstance();
        Thread.Sleep(100);
    }

    [Fact]
    public void UserTest_TestPermissionHappyPath()
    {
        Setup();
        Assert.Equal(UserOperationResult.Ok, SuperUser.CreateEntity(EntityType.Project, ProjectId));
        Assert.Equal(UserOperationResult.Ok, SuperUser.CreateEntity(EntityType.Task, TaskId));
        Assert.Equal(UserOperationResult.Ok, SuperUser.ChangeEntityProperty(EntityType.Project, ProjectId ,EntityProperty.Description, "TestDescription"));
        Assert.Equal(UserOperationResult.Ok, SuperUser.ChangeEntityProperty(EntityType.Task, TaskId ,EntityProperty.EntityStatus, EntityStatus.InProgress));
        Assert.Equal(UserOperationResult.Ok, SuperUser.ChangeEntityProperty(EntityType.Project, ProjectId ,EntityProperty.EntityPriority, EntityPriority.High));
        Assert.Equal(UserOperationResult.Ok, SuperUser.ChangeEntityProperty(EntityType.Task, TaskId ,EntityProperty.StartedDate, DateTime.Today));
        Assert.Equal(UserOperationResult.Ok, SuperUser.ChangeEntityProperty(EntityType.Task, TaskId ,EntityProperty.FinishedDate,  DateTime.Today));
    }
}