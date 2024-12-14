using System;
using Xunit;
using PMSCore;

namespace PMSCore.Tests
{
    /// <summary>
    /// Unit tests for the ReportManager class.
    /// </summary>
    public class ReportManagerTests
    {
        /// <summary>
        /// A basic mock implementation of ILogger for testing purposes.
        /// </summary>
        private class MockLogger : ILogger
        {
            public void Log(string message)
            {
                // Mock logger does nothing with the message.
            }
        }

        /// <summary>
        /// Tests that GetInstance always returns the same instance of ReportManager.
        /// </summary>
        [Fact]
        public void GetInstance_ReturnsSameInstance()
        {
            // Arrange: Create a mock logger to pass to the method.
            var logger = new MockLogger();

            // Act: Get two instances of ReportManager.
            var instance1 = ReportManager.GetInstance(logger);
            var instance2 = ReportManager.GetInstance(logger);

            // Assert: The two instances should be the same.
            Assert.Same(instance1, instance2);
        }

        /// <summary>
        /// Tests that ResetInstance allows a new instance of ReportManager to be created.
        /// </summary>
        [Fact]
        public void ResetInstance_CreatesNewInstance()
        {
            // Arrange: Create a mock logger and get the current instance.
            var logger = new MockLogger();
            var oldInstance = ReportManager.GetInstance(logger);

            // Act: Reset the instance and create a new one.
            ReportManager.ResetInstance();
            var newInstance = ReportManager.GetInstance(logger);

            // Assert: The old and new instances should be different.
            Assert.NotSame(oldInstance, newInstance);
        }

        /// <summary>
        /// Tests that GetInstance uses a default logger if null is passed.
        /// </summary>
        [Fact]
        public void GetInstance_UsesDefaultLoggerIfNull()
        {
            // Act: Call GetInstance with a null logger.
            var reportManager = ReportManager.GetInstance(null);

            // Assert: The instance should be created successfully (no exception thrown).
            Assert.NotNull(reportManager);
        }

        /// <summary>
        /// Tests that GenerateProjectReport throws an exception if the project is null.
        /// </summary>
        [Fact]
        public void GenerateProjectReport_ThrowsExceptionIfProjectIsNull()
        {
            // Arrange: Create a mock logger and get the ReportManager instance.
            var logger = new MockLogger();
            var reportManager = ReportManager.GetInstance(logger);
            Project nullProject = null;

            // Act and Assert: Calling the method with a null project should throw an exception.
            var exception = Assert.Throws<ArgumentNullException>(() => reportManager.GenerateProjectReport(nullProject));
            Assert.Equal("Project cannot be null. (Parameter 'project')", exception.Message);
        }

        /// <summary>
        /// Tests that GenerateTaskReport throws an exception if the task is null.
        /// </summary>
        [Fact]
        public void GenerateTaskReport_ThrowsExceptionIfTaskIsNull()
        {
            // Arrange: Create a mock logger and get the ReportManager instance.
            var logger = new MockLogger();
            var reportManager = ReportManager.GetInstance(logger);
            Task nullTask = null;

            // Act and Assert: Calling the method with a null task should throw an exception.
            var exception = Assert.Throws<ArgumentNullException>(() => reportManager.GenerateTaskReport(nullTask));
            Assert.Equal("Task cannot be null. (Parameter 'task')", exception.Message);
        }

        /// <summary>
        /// Tests that GenerateProjectReport returns the correct report for a valid project.
        /// </summary>
        [Fact]
        public void GenerateProjectReport_ReturnsCorrectReportForValidProject()
        {
            // Arrange: Create a mock logger and a mock project with a report.
            var logger = new MockLogger();
            var reportManager = ReportManager.GetInstance(logger);
            var mockProject = new MockProject("Test Project Report");

            // Act: Call GenerateProjectReport with the mock project.
            var report = reportManager.GenerateProjectReport(mockProject);

            // Assert: The report should match the mock project's report.
            Assert.Equal("Test Project Report", report);
        }

        /// <summary>
        /// Tests that GenerateTaskReport returns the correct report for a valid task.
        /// </summary>
        [Fact]
        public void GenerateTaskReport_ReturnsCorrectReportForValidTask()
        {
            // Arrange: Create a mock logger and a mock task with a report.
            var logger = new MockLogger();
            var reportManager = ReportManager.GetInstance(logger);
            var mockTask = new MockTask("Test Task Report");

            // Act: Call GenerateTaskReport with the mock task.
            var report = reportManager.GenerateTaskReport(mockTask);

            // Assert: The report should match the mock task's report.
            Assert.Equal("Test Task Report", report);
        }

        /// <summary>
        /// A simple mock of the Project class used for testing.
        /// </summary>
        private class MockProject : Project
        {
            private readonly string _report;

            public MockProject(string report)
            {
                _report = report;
            }

            public override string GenerateReport()
            {
                return _report;
            }
        }

        /// <summary>
        /// A simple mock of the Task class used for testing.
        /// </summary>
        private class MockTask : Task
        {
            private readonly string _report;

            public MockTask(string report)
            {
                _report = report;
            }

            public override string GenerateReport()
            {
                return _report;
            }
        }
    }
}