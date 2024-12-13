namespace PMSCore.Test;

public class TaskManagerTest
{
    private static string TaskId { get; } = "TestTask";
    private static string UserId { get; } = "TestUser";
    private static string ProjectId { get; } = "TestProject";
    
    /// <summary>
    /// Tests Task creation happy path
    /// </summary>
    [Fact]
    public void TaskManagerTest_CreateTaskHappyPath()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.True(tm.CreateTask(TaskId));
    }
    
    /// <summary>
    /// Tests Task to User association happy path
    /// </summary>
    [Fact]
    public void TaskManagerTest_AssociateTaskToUserHappyPath()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.Equal(AssociationStatus.NoError, tm.AssociateTaskToUser(TaskId, UserId));
    }
    
    /// <summary>
    /// Tests Task to Project association happy path
    /// </summary>
    [Fact]
    public void TaskManagerTest_AssociateTaskToProjectHappyPath()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.Equal(AssociationStatus.NoError, tm.AssociateTaskToProject(TaskId, ProjectId));
    }
    
    /// <summary>
    /// Tests Task to User disassociation happy path
    /// </summary>
    [Fact]
    public void TaskManagerTest_RemoveTaskFromUserHappyPath()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.Equal(AssociationStatus.NoError, tm.RemoveTaskFromUser(TaskId, UserId));
    }
    
    /// <summary>
    /// Tests Task to Project disassociation happy path
    /// </summary>
    [Fact]
    public void TaskManagerTest_RemoveTaskFromProjectHappyPath()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.Equal(AssociationStatus.NoError, tm.RemoveTaskFromProject(TaskId, ProjectId));
    }
}