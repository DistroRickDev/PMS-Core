namespace PMSCore.Test
{
    /// <summary>
    /// Unit tests for the Tester class.
    /// </summary>
    public class TesterTest
    {
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
        public void AddTestCase_AddsAValidTestCaseToList()
        {
            // Arrange
            var tester = new Tester("Tester1");

            // Act
            tester.AddTestCase("Test case 1");

            // Assert
            Assert.Contains("Test case 1", tester.TestCases);
        }

        /// <summary>
        /// Ensures an invalid (empty) test case is not added.
        /// </summary>
        [Fact]
        public void AddTestCase_ShowsErrorForInvalidTestCase()
        {
            // Arrange
            var tester = new Tester("Tester1");

            // Act
            tester.AddTestCase("");

            // Assert
            Assert.Empty(tester.TestCases);
        }

        /// <summary>
        /// Verifies that all test cases are displayed correctly.
        /// </summary>
        [Fact]
        public void ViewTestCases_ShouldShowAllTestCases()
        {
            // Arrange
            var tester = new Tester("Tester1");
            tester.AddTestCase("Test case 1");
            tester.AddTestCase("Test case 2");

            // Act
            var consoleOutput = CaptureConsoleOutput(() => tester.ViewTestCases());

            // Assert
            Assert.Contains("Test case 1", consoleOutput);
            Assert.Contains("Test case 2", consoleOutput);
        }

        /// <summary>
        /// Ensures a valid bug report is added to the list.
        /// </summary>
        [Fact]
        public void SubmitBugReport_AValidReport_AddsToTheBugReports()
        {
            // Arrange
            var tester = new Tester("Tester1");

            // Act
            tester.SubmitBugReport("Bug 1", "High");

            // Assert
            Assert.Single(tester.BugReports);
            Assert.Contains(("Bug 1", "High"), tester.BugReports);
        }

        /// <summary>
        /// Ensures invalid bug reports (empty values) are not added.
        /// </summary>
        [Fact]
        public void SubmitBugReport_InvalidReport_ShowsError()
        {
            // Arrange
            var tester = new Tester("Tester1");

            // Act
            tester.SubmitBugReport("", "");

            // Assert
            Assert.Empty(tester.BugReports);
        }

        /// <summary>
        /// Verifies that all submitted bug reports are displayed.
        /// </summary>
        [Fact]
        public void ViewBugReports_ShowsAllAvailableBugReports()
        {
            // Arrange
            var tester = new Tester("Tester1");
            tester.SubmitBugReport("Bug 1", "High");
            tester.SubmitBugReport("Bug 2", "Low");

            // Act
            var consoleOutput = CaptureConsoleOutput(() => tester.ViewBugReports());

            // Assert
            Assert.Contains("Bug 1 (Severity: High)", consoleOutput);
            Assert.Contains("Bug 2 (Severity: Low)", consoleOutput);
        }

        /// <summary>
        /// Utility method to capture and return console output for testing.
        /// </summary>
        private string CaptureConsoleOutput(Action action)
        {
            var originalConsoleOut = Console.Out;
            using (var consoleOutput = new System.IO.StringWriter())
            {
                Console.SetOut(consoleOutput);
                action.Invoke();
                Console.SetOut(originalConsoleOut);
                return consoleOutput.ToString();
            }
        }
    }
}