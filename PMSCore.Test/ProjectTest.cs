using Microsoft.Extensions.Logging;
using PMSCore.Test;

namespace PMSCore.Test
{
    /// <summary>
    /// Unit tests for the <see cref="Project"/> class.
    /// </summary>
    public class ProjectTest
    {
        private readonly ILogger _testLogger = new LoggerFake();

        /// <summary>
        /// Checks if the project is created with the right values.
        /// </summary>
        [Fact]
        public void ProjectTest_ShouldInitializePropertiesCorrectly()
        {
            // Arrange: Set up the details for a new project.
            string id = "Test Project";
            string description = "A sample project for testing.";

            // Act: Create a project using the constructor.
            var project = Project.CreateProject(_testLogger, id, description);

            // Assert: Verify that the project properties match the inputs.
            Assert.NotNull(project);
            Assert.Equal(id, project!.GetId());
            Assert.Equal(description, project!.GetDescription());
            Assert.Equal(DateTime.Now.Date, project!.GetCreatedDate().Date);
        }

        /// <summary>
        /// Checks if adding a task updates the task list and logs the action.
        /// </summary>
        [Fact]
        public void ProjectTest_ShouldUpdatePropertiesCorrectly()
        {
            // Arrange: Create a project and a task to add to it.
            var project = Project.CreateProject(_testLogger, "Test Project", "description");
            var expectedDate = DateTime.Now;
            var expectedFinishedDate = DateTime.Today.AddDays(2);
            Assert.NotNull(project);
            project!.SetDescription("New Description");
            project!.SetStatus(EntityStatus.Done);
            project!.SetPriority(EntityPriority.Critical);
            project!.SetStartedDate(expectedDate);
            project!.SetFinishedDate(expectedFinishedDate);
            Assert.Equal("New Description", project!.GetDescription());
            Assert.Equal(EntityStatus.Done, project!.GetStatus());
            Assert.Equal(EntityPriority.Critical, project!.GetPriority());
            Assert.Equal(expectedDate, project!.GetStartedDate());
            Assert.Equal(expectedFinishedDate, project!.GetFinishedDate());
        }

        /// <summary>
        /// Checks if the action log is updated when a project is created.
        /// </summary>
        [Fact]
        public void ProjectTest_ShouldUpdateActionLog()
        {
            // Arrange: Prepare inputs for creating a new project.
            var project = Project.CreateProject(_testLogger, "Test Project", "description");
            Assert.NotNull(project);
            project!.SetDescription("New Description");
            Assert.Contains("Changed description from 'description' to 'New Description'", project!.GetReport());
        }

        /// <summary>
        /// Ensures the DisplayDetails method runs without errors.
        /// </summary>
        [Fact]
        public void ProjectTest_ShouldPrintProjectDetails()
        {
            var project = Project.CreateProject(_testLogger, "Test Project", "description");
            Assert.NotNull(project);
            Assert.Contains(
                $"Id: Test Project\nDescription: description\nPriority: {EntityPriority.Medium}\nStatus: {EntityStatus.New}",
                project!.DisplayEntityDetails());
        }

        [Fact]
        public void ProjectTest_FailedToCreateProject()
        {
            var project = Project.CreateProject(_testLogger, null, "description");
            Assert.Null(project);
            Assert.Contains("Failed to create project reason: empty id", (_testLogger as LoggerFake)!.LogStream);
        }
    }
}