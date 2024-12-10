using System;
using PMSCore;

namespace PMSCore
{
    /// <summary>
    /// This class is used to handle and create reports.
    /// </summary>
    public class ReportManager
    {
        /// <summary>
        /// Makes a report from a project.
        /// </summary>
        /// <param name="project">The project to get the report from.</param>
        /// <returns>A string that has the project report.</returns>
        public string GenerateProjectReport(Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project), "Project cannot be null.");
            }

            return project.GenerateReport();
        }

        /// <summary>
        /// Makes a report from a task.
        /// </summary>
        /// <param name="task">The task to get the report from.</param>
        /// <returns>A string that has the task report.</returns>
        public string GenerateTaskReport(Task task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task), "Task cannot be null.");
            }

            return task.GenerateReport();
        }
    }
}