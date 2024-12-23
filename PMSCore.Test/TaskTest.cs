using System.Text.Json;
using Microsoft.Extensions.Logging;
using PMSCore.Test;
using Xunit.Abstractions;

namespace PMSCore.Test
{
    /// <summary>
    /// Unit tests for the <see cref="Task"/> class.
    /// </summary>
    public class TaskTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly ILogger _testLogger = new LoggerFake();

        public TaskTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Checks if the Task is created with the right values.
        /// </summary>
        [Fact]
        public void TaskTest_ShouldInitializePropertiesCorrectly()
        {
            // Arrange: Set up the details for a new Task.
            string id = "Test Task";
            string description = "A sample Task for testing.";

            // Act: Create a Task using the constructor.
            var task = Task.CreateTask(_testLogger, id, description);

            // Assert: Verify that the Task properties match the inputs.
            Assert.NotNull(task);
            Assert.Equal(id, task!.GetId());
            Assert.Equal(description, task!.GetDescription());
            Assert.Equal(DateTime.Now.Date, task!.GetCreatedDate().Date);
        }

        /// <summary>
        /// Checks if adding a task updates the task list and logs the action.
        /// </summary>
        [Fact]
        public void TaskTest_ShouldUpdatePropertiesCorrectly()
        {
            // Arrange: Create a Task and a task to add to it.
            var task = Task.CreateTask(_testLogger, "Test Task", "description");
            var expectedDate = DateTime.Now;
            var expectedFinishedDate = DateTime.Today.AddDays(2);
            Assert.NotNull(task);
            task!.SetDescription("New Description");
            task!.SetStatus(EntityStatus.Done);
            task!.SetPriority(EntityPriority.Critical);
            task!.SetStartedDate(expectedDate);
            task!.SetFinishedDate(expectedFinishedDate);
            Assert.Equal("New Description", task!.GetDescription());
            Assert.Equal(EntityStatus.Done, task!.GetStatus());
            Assert.Equal(EntityPriority.Critical, task!.GetPriority());
            Assert.Equal(expectedDate, task!.GetStartedDate());
            Assert.Equal(expectedFinishedDate, task!.GetFinishedDate());
        }

        /// <summary>
        /// Checks if the action log is updated when a Task is created.
        /// </summary>
        [Fact]
        public void TaskTest_ShouldUpdateActionLog()
        {
            // Arrange: Prepare inputs for creating a new Task.
            var task = Task.CreateTask(_testLogger, "Test Task", "description");
            Assert.NotNull(task);
            task!.SetDescription("New Description");
            Assert.Contains("Changed description from 'description' to 'New Description'", task!.GetReport());
        }

        /// <summary>
        /// Ensures the DisplayDetails method runs without errors.
        /// </summary>
        [Fact]
        public void TaskTest_ShouldPrintTaskDetails()
        {
            var task = Task.CreateTask(_testLogger, "Test Task", "description");
            Assert.NotNull(task);
            Assert.Contains(
                $"Id: Test Task\nDescription: description\nPriority: {EntityPriority.Medium}\nStatus: {EntityStatus.New}",
                task!.DisplayEntityDetails());
        }

        [Fact]
        public void TaskTest_FailedToCreateTask()
        {
            var task = Task.CreateTask(_testLogger, null, "description");
            Assert.Null(task);
            Assert.Contains("Failed to create Task reason: empty id", (_testLogger as LoggerFake)!.LogStream);
        }

        [Fact]
        public void TaskTest_JsonTest()
        {
            Entity task = Task.CreateTask(_testLogger, "ToBeSerializedId", "description")!;
            var jsonObj = JsonSerializer.Serialize(task);
            Assert.NotNull(jsonObj);
            _testOutputHelper.WriteLine(jsonObj);
            var desTask = JsonSerializer.Deserialize<Entity>(jsonObj);
            Assert.NotNull(desTask);
            Assert.Equal("ToBeSerializedId", desTask!.GetId());
            Assert.Equal("description", desTask.GetDescription());
        }
    }
}