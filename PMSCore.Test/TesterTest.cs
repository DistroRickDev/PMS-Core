namespace PMSCore.Test
{
    /// <summary>
    /// Unit tests for the Tester class.
    /// </summary>
    public class TesterTest
    {
        private readonly Mock<IEntityManager> _entityManagerMock;
        private readonly Mock<IStateManager> _stateManagerMock;

        public TesterTest()
        {
            _entityManagerMock = new Mock<IEntityManager>();
            _stateManagerMock = new Mock<IStateManager>();
            EntityManager.SetInstance(_entityManagerMock.Object);
            StateManager.SetInstance(_stateManagerMock.Object);
        }

        /// <summary>
        /// Ensures the Tester constructor initializes with default values.
        /// </summary>
        [Fact]
        public void Constructor_InitializesATesterWithDefaultParameters()
        {
            // Arrange + Act
            var tester = new Tester("Tester1");

            // Assert
            Assert.Equal("Tester1", tester.UserName);
            Assert.Equal(Permission.TESTER, tester.GetPermission());
            Assert.NotNull(tester); // Verify the tester object is created.
        }

        /// <summary>
        /// Verifies that a valid test case is added successfully.
        /// </summary>
        [Fact]
        public void AddTestCase_CreatesEntityForValidTestCase()
        {
            // Arrange
            var tester = new Tester("Tester1");
            var testCaseDescription = "Test case 1";

            // Act
            tester.AddTestCase(testCaseDescription);

            // Assert
            _entityManagerMock.Verify(em => em.CreateInstance(
                It.IsAny<string>(),
                testCaseDescription,
                EntityType.Task), Times.Once);
        }

        /// <summary>
        /// Ensures an invalid (empty) test case is not added.
        /// </summary>
        [Fact]
        public void AddTestCase_DoesNotCreateEntityForInvalidTestCase()
        {
            // Arrange
            var tester = new Tester("Tester1");

            // Act
            tester.AddTestCase("");

            // Assert
            _entityManagerMock.Verify(em => em.CreateInstance(
                It.IsAny<string>(),
                It.IsAny<string>(),
                EntityType.Task), Times.Never);
        }

        /// <summary>
        /// Verifies that all test cases are retrieved through StateManager.
        /// </summary>
        [Fact]
        public void GetTestCases_ReturnsEntitiesFromStateManager()
        {
            // Arrange
            var tester = new Tester("Tester1");
            var testEntities = new List<Entity>
            {
                new Entity { Id = "1", Name = "Test case 1", Type = EntityType.Task },
                new Entity { Id = "2", Name = "Test case 2", Type = EntityType.Task }
            };
            _stateManagerMock.Setup(sm => sm.GetEntitiesByUser(tester, EntityType.Task))
                .Returns(testEntities);

            // Act
            var result = tester.GetTestCases();

            // Assert
            Assert.Equal(testEntities, result);
        }

        /// <summary>
        /// Verifies that a valid bug report is added successfully.
        /// </summary>
        [Fact]
        public void SubmitBugReport_CreatesEntityForValidBugReport()
        {
            // Arrange
            var tester = new Tester("Tester1");
            var description = "Bug 1";
            var severity = "High";

            // Act
            tester.SubmitBugReport(description, severity);

            // Assert
            _entityManagerMock.Verify(em => em.CreateInstance(
                It.IsAny<string>(),
                $"{description} - {severity}",
                EntityType.Task), Times.Once);
        }

        /// <summary>
        /// Ensures an invalid bug report is not added.
        /// </summary>
        [Fact]
        public void SubmitBugReport_DoesNotCreateEntityForInvalidBugReport()
        {
            // Arrange
            var tester = new Tester("Tester1");

            // Act
            tester.SubmitBugReport("", "");

            // Assert
            _entityManagerMock.Verify(em => em.CreateInstance(
                It.IsAny<string>(),
                It.IsAny<string>(),
                EntityType.Task), Times.Never);
        }

        /// <summary>
        /// Verifies that all bug reports are retrieved through StateManager.
        /// </summary>
        [Fact]
        public void GetBugReports_ReturnsEntitiesFromStateManager()
        {
            // Arrange
            var tester = new Tester("Tester1");
            var bugEntities = new List<Entity>
            {
                new Entity { Id = "1", Name = "Bug 1 - High", Type = EntityType.Task },
                new Entity { Id = "2", Name = "Bug 2 - Low", Type = EntityType.Task }
            };
            _stateManagerMock.Setup(sm => sm.GetEntitiesByUser(tester, EntityType.Task))
                .Returns(bugEntities);

            // Act
            var result = tester.GetBugReports();

            // Assert
            Assert.Equal(bugEntities, result);
        }
    }
}