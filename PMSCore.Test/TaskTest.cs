namespace PMSCore;

public class TaskTest
{
    [Fact]
    public void ConstructorShouldCreateTaskWithValidParameters()
    {
        // Using AAA pattern for arranging unit tests
        
        // Arrange - set up the testing objects
        // Act - perform the action
        // Assert - verify the result

        // Arrange

        var taskName = "Testing task";
        var description = "A sample description";
        var priority = TaskPriority.Low;

        // Act
        var task1 = new Task(taskName, description, priority);

        // Assert
        Assert.Equal(taskName, task1.TaskName);
        Assert.Equal(description, task1.Description);
        Assert.Equal(priority, task1.Priority);
        Assert.Equal(TaskStatus.New, task1.Status);

    }

    [Theory]
    [InlineData(TaskStatus.New)]
    [InlineData(TaskStatus.InProgress)]
    [InlineData(TaskStatus.Completed)]
    [InlineData(TaskStatus.Blocked)]
    public void ChangeTaskStatus_ShouldChangeStatus(TaskStatus newStatus)
    {
        // Arrange
        var task2 = new Task("Testing task 2", "Another description", TaskPriority.Medium);

        // Act
        task2.ChangeTaskStatus(newStatus);

        // Assert
        Assert.Equal(newStatus, task2.Status);

    }

    [Fact]
    public void ActionRequired_ReturnsTrue_IfTaskNotComplete()
    {
        // Arrange
        var task3 = new Task("Testing task 3", "Third description", TaskPriority.High);

        // Act
        var result = task3.ActionRequired();

        // Assert
        Assert.True(result);

    }

    [Fact]
    public void ActionRequired_ReturnsFalse_IfTaskIsComplete()
    {
        // Arrange
        var task4 = new Task("Testing task 4", "Fourth description", TaskPriority.Low);
        task4.ChangeTaskStatus(TaskStatus.Completed);

        // Act
        var result = task4.ActionRequired();

        // Assert
        Assert.False(result);
    }


}
