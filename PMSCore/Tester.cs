namespace PMSCore
{
    /// <summary>
    /// A Tester user who manages test cases and bug reporting.
    /// </summary>
    public class Tester : UserBase
    {
        // List to store test case descriptions
        public List<string> TestCases { get; private set; }

        // List to store bug reports with description and severity
        public List<(string Description, string Severity)> BugReports { get; private set; }

        /// <summary>
        /// Initializes a Tester user with a username and TESTER permission level.
        /// </summary>
        public Tester(string userName) : base(userName, Permission.TESTER)
        {
            TestCases = new List<string>();
            BugReports = new List<(string Description, string Severity)>();
        }

        /// <summary>
        /// Adds a new test case if the description is valid.
        /// </summary>
        /// <param name="testCaseDescription">Description of the test case.</param>
        public void AddTestCase(string testCaseDescription)
        {
            if (!string.IsNullOrWhiteSpace(testCaseDescription))
            {
                TestCases.Add(testCaseDescription);
                // Replace Console.WriteLine with logging for better testability
                _logger.LogInformation($"Test case added: {testCaseDescription}");
            }
            else
            {
                _logger.LogWarning("Test case description should not be empty.");
            }
        }

        /// <summary>
        /// Returns the list of test cases added by the tester.
        /// </summary>
        /// <returns>List of test case descriptions.</returns>
        public List<string> GetTestCases()
        {
            return new List<string>(TestCases);
        }

        /// <summary>
        /// Submits a new bug report if the description and severity are valid.
        /// </summary>
        /// <param name="description">Description of the bug.</param>
        /// <param name="severity">Severity level of the bug.</param>
        public void SubmitBugReport(string description, string severity)
        {
            if (!string.IsNullOrWhiteSpace(description) && !string.IsNullOrWhiteSpace(severity))
            {
                BugReports.Add((description, severity));
                _logger.LogInformation($"Bug report submitted: {description} (Severity: {severity})");
            }
            else
            {
                _logger.LogWarning("Description and severity for a bug report must not be empty!");
            }
        }

        /// <summary>
        /// Returns all submitted bug reports.
        /// </summary>
        /// <returns>List of bug reports with descriptions and severity.</returns>
        public List<(string Description, string Severity)> GetBugReports()
        {
            return new List<(string Description, string Severity)>(BugReports);
        }
    }
}