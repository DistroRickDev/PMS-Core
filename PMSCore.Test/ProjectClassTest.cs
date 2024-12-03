using System;
using Xunit;
using PMSCore;

namespace PMSCore.Tests
{
    /// <summary>
    /// Unit tests for the <see cref="Project"/> class.
    /// </summary>
    public class ProjectTests
    {
        /// <summary>
        /// Checks if the project is created with the right values.
        /// </summary>
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Arrange: Set up the details for a new project.
            string name = "Test Project";
            string description = "A sample project for testing.";
            DateTime deadline = DateTime.Now.AddDays(10);

            // Act: Create a project using the constructor.
            var project = new Project(name, description, deadline);

            // Assert: Verify that the project properties match the inputs.
            Assert.Equal(name, project.Name);
            Assert.Equal(description, project.Description);
            Assert.Equal(deadline, project.Deadline);
            Assert.Equal(ProjectStatus.NotStarted, project.Status);
        }

        /// <summary>
        /// Checks if adding a task updates the task list and logs the action.
        /// </summary>
        [Fact]
        public void AddTask_ShouldUpdateTaskListAndLog()
        {
            // Arrange: Create a project and a task to add to it.
            var project = new Project("Test Project", "A sample project.", DateTime.Now.AddDays(5));
            var task = new Task();

            // Act: Add the task to the project.
            project.AddTask(task);

            // Assert: Confirm the task is added to the list and logged correctly.
            Assert.Single(project.Tasks);
            Assert.Contains("Task added", project.GenerateReport());
        }

        /// <summary>
        /// Checks if the action log is updated when a project is created.
        /// </summary>
        [Fact]
        public void Constructor_ShouldUpdateActionLog()
        {
            // Arrange: Prepare inputs for creating a new project.
            string name = "Log Test Project";
            string description = "Testing action log.";
            DateTime deadline = DateTime.Now.AddDays(15);

            // Act: Create a new project.
            var project = new Project(name, description, deadline);

            // Assert: Confirm the action log contains the creation entry.
            Assert.Contains("Project created", project.GenerateReport());
        }

        /// <summary>
        /// Ensures the DisplayDetails method runs without errors.
        /// </summary>
        [Fact]
        public void DisplayDetails_ShouldPrintProjectDetails()
        {
            // Arrange: Create a project and add a task for testing the display.
            var project = new Project("Display Test Project", "Testing display.", DateTime.Now.AddDays(20));
            var task = new Task();
            project.AddTask(task);

            // Act: Call the DisplayDetails method to print details.
            var exception = Record.Exception(() => project.DisplayDetails());

            // Assert: Verify no exceptions occur during the method execution.
            Assert.Null(exception);
        }
    }
}