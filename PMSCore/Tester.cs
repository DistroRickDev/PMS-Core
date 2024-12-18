namespace PMSCore
{
    /// <summary>
    /// A Tester user who manages test cases and bug reporting.
    /// </summary>
    public class Tester : UserBase
    {
        /// <summary>
        /// Initializes a Tester user with a username and TESTER permission level.
        /// </summary>
        public Tester(string userName) : base(userName, Permission.TESTER)
        {
        }

        /// <summary>
        /// Adds a new test case as an Entity of type Task through the EntityManager.
        /// </summary>
        /// <param name="testCaseDescription">Description of the test case.</param>
        public void AddTestCase(string testCaseDescription)
        {
            if (!string.IsNullOrWhiteSpace(testCaseDescription))
            {
                EntityManager.GetInstance().CreateInstance("TestId", testCaseDescription, EntityType.Task);
                _logger.LogInformation($"Test case added: {testCaseDescription}");
            }
            else
            {
                _logger.LogWarning("Test case description should not be empty.");
            }
        }

        /// <summary>
        /// Returns all test cases associated with the tester.
        /// </summary>
        /// <returns>List of test cases.</returns>
        public List<Entity> GetTestCases()
        {
            return StateManager.GetInstance().GetEntitiesByUser(this, EntityType.Task);
        }

        /// <summary>
        /// Submits a new bug report as an Entity of type Task through the EntityManager.
        /// </summary>
        /// <param name="description">Description of the bug.</param>
        /// <param name="severity">Severity level of the bug.</param>
        public void SubmitBugReport(string description, string severity)
        {
            if (!string.IsNullOrWhiteSpace(description) && !string.IsNullOrWhiteSpace(severity))
            {
                EntityManager.GetInstance().CreateInstance("BugId", $"{description} - {severity}", EntityType.Task);
                _logger.LogInformation($"Bug report submitted: {description} (Severity: {severity})");
            }
            else
            {
                _logger.LogWarning("Description and severity for a bug report must not be empty!");
            }
        }

        /// <summary>
        /// Returns all submitted bug reports associated with the tester.
        /// </summary>
        /// <returns>List of bug reports.</returns>
        public List<Entity> GetBugReports()
        {
            return StateManager.GetInstance().GetEntitiesByUser(this, EntityType.Task);
        }
    }
}