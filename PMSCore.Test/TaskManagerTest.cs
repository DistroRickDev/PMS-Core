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
    
    /// <summary>
    /// Tests Task to User association invalid taskId
    /// </summary>
    [Fact]
    public void TaskManagerTest_AssociateTaskToUserInvalidTaskId()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.Equal(AssociationStatus.InvalidTask, tm.AssociateTaskToUser(null, UserId));
    }
    
    /// <summary>
    /// Tests Task to User association invalid userId
    /// </summary>
    [Fact]
    public void TaskManagerTest_AssociateTaskToUserInvalidUserId()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.Equal(AssociationStatus.InvalidUser, tm.AssociateTaskToUser(TaskId, null));
    }
    
    /// <summary>
    /// Tests Task to Project association invalid TaskId
    /// </summary>
    [Fact]
    public void TaskManagerTest_AssociateTaskToProjectInvalidTaskId()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.Equal(AssociationStatus.InvalidTask, tm.AssociateTaskToProject(null, ProjectId));
    }
    
    /// <summary>
    /// Tests Task to Project association invalid userId
    /// </summary>
    [Fact]
    public void TaskManagerTest_AssociateTaskToProjectInvalidUserId()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.Equal(AssociationStatus.InvalidProject, tm.AssociateTaskToProject(TaskId, null));
    }
    
    /// <summary>
    /// Tests Task to User disassociation invalid taskId
    /// </summary>
    [Fact]
    public void TaskManagerTest_RemoveTaskFromUserInvalidTaskId()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.Equal(AssociationStatus.InvalidTask, tm.RemoveTaskFromUser(null, UserId));
    }
    
    /// <summary>
    /// Tests Task to User disassociation invalid userId
    /// </summary>
    [Fact]
    public void TaskManagerTest_RemoveTaskFromUserInvalidUserId()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.Equal(AssociationStatus.InvalidUser, tm.RemoveTaskFromUser(TaskId, null));
    }
    
    /// <summary>
    /// Tests Task to Project disassociation invalid TaskId
    /// </summary>
    [Fact]
    public void TaskManagerTest_RemoveTaskFromProjectInvalidTaskId()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.Equal(AssociationStatus.InvalidTask, tm.RemoveTaskFromProject(null, ProjectId));
    }
    
    /// <summary>
    /// Tests Task to Project disassociation invalid userId
    /// </summary>
    [Fact]
    public void TaskManagerTest_RemoveTaskFromProjectInvalidUserId()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.Equal(AssociationStatus.InvalidProject, tm.RemoveTaskFromProject(TaskId, null));
    }
    
    /// <summary>
    /// Tests Task change Task Name Happy path
    /// </summary>
    [Fact]
    public void TaskManagerTest_ChangeTaskNameHappyPath()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.True(tm.ChangeTaskName(TaskId, "TestTask"));
    }
    
    /// <summary>
    /// Tests Task change Task Name invalid TaskId
    /// </summary>
    [Fact]
    public void TaskManagerTest_ChangeTaskNameInvalidTaskId()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.False(tm.ChangeTaskName(null, "TestTask"));
    }
    
    /// <summary>
    /// Tests Task change Task Name invalid name
    /// </summary>
    [Fact]
    public void TaskManagerTest_ChangeTaskNameInvalidName()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.False(tm.ChangeTaskName(TaskId, string.Empty));
    }
    
    /// <summary>
    /// Tests Task change Task Description Happy path
    /// </summary>
    [Fact]
    public void TaskManagerTest_ChangeTaskDescriptionHappyPath()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.True(tm.ChangeTaskDescription(TaskId, "TestDescription"));
    }
    
    /// <summary>
    /// Tests Task change Task Description invalid TaskId
    /// </summary>
    [Fact]
    public void TaskManagerTest_ChangeTaskDescriptionInvalidTaskId()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.False(tm.ChangeTaskDescription(null, "TestDescription"));
    }

    /// <summary>
    /// Tests Task change Task Status Happy path
    /// </summary>
    [Fact]
    public void TaskManagerTest_ChangeTaskStatusHappyPath()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.True(tm.ChangeTaskStatus(TaskId, TaskStatus.Completed));
    }
    
    /// <summary>
    /// Tests Task change Task Status invalid TaskId
    /// </summary>
    [Fact]
    public void TaskManagerTest_ChangeTaskStatusInvalidTaskId()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.False(tm.ChangeTaskStatus(null, TaskStatus.Completed));
    }
    
    /// <summary>
    /// Tests Task change Task Priority Happy path
    /// </summary>
    [Fact]
    public void TaskManagerTest_ChangeTaskPriorityHappyPath()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.True(tm.ChangeTaskPriority(TaskId, TaskPriority.Critical));
    }
    
    /// <summary>
    /// Tests Task change Task Priority invalid TaskId
    /// </summary>
    [Fact]
    public void TaskManagerTest_ChangeTaskStatusPriorityTaskId()
    {
        TaskManager.ResetInstance();
        var tm = TaskManager.GetInstance();
        Assert.False(tm.ChangeTaskPriority(null, TaskPriority.Critical));
    }
 
}