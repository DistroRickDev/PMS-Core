using System;
using PMSCore;

namespace PMSCore
{
    /// <summary>
    /// This class handles the creation and management of reports.
    /// </summary>
    public sealed class ReportManager
    {
        // The single instance of ReportManager
        private static ReportManager _instance;

        // Logger instance
        private readonly ILogger _logger;

        /// <summary>
        /// Private constructor to prevent external instantiation.
        /// </summary>
        /// <param name="logger">The logger instance to be used for logging.</param>
        private ReportManager(ILogger logger)
        {
            // Use the provided logger or fallback to the factory with a meaningful name
            _logger = logger ?? LoggerFactory.CreateLogger("ReportManager");

            // Log the creation of the ReportManager instance
            _logger.Log("ReportManager instance created.");
        }

        /// <summary>
        /// Provides access to the single instance of ReportManager.
        /// </summary>
        /// <param name="logger">The logger instance to be passed if the instance needs to be created.</param>
        /// <returns>The single instance of ReportManager.</returns>
        public static ReportManager GetInstance(ILogger logger)
        {
            if (_instance == null)
            {
                _instance = new ReportManager(logger);
            }

            return _instance;
        }

        /// <summary>
        /// Resets the instance of ReportManager for reinitialization.
        /// </summary>
        public static void ResetInstance()
        {
            _instance = null;
        }

        /// <summary>
        /// Creates a report from the provided project.
        /// </summary>
        /// <param name="project">The project to create the report from.</param>
        /// <returns>A string containing the project's report.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the project is null.</exception>
        public string GenerateProjectReport(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

            _logger.Log("Generating project report.");
            return project.GenerateReport();
        }

        /// <summary>
        /// Creates a report from the provided task.
        /// </summary>
        /// <param name="task">The task to create the report from.</param>
        /// <returns>A string containing the task's report.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the task is null.</exception>
        public string GenerateTaskReport(Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "Task cannot be null.");
            }

            _logger.Log("Generating task report.");
            return task.GenerateReport();
        }
    }
}