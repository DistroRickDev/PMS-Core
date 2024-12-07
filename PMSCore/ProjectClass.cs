using System;
using System.Collections.Generic;

namespace PMSCore
{
    /// <summary>
    /// Status of a project.
    /// </summary>
    public enum ProjectStatus { NotStarted, InProgress, Completed }

    /// <summary>
    /// A project with tasks and deadlines.
    /// </summary>
    public class Project
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; }
        public DateTime Deadline { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.NotStarted;
        public List<Task> Tasks { get; } = new List<Task>();
        private string report = "";

        private void addToReport(string message)
        {
            report += $"[ActionLog] {DateTime.Now.ToString()} : {message}\n";
        }

        /// <summary>
        /// Creates a project with a name, description, and deadline.
        /// </summary>
        public Project(string name, string description, DateTime deadline)
        {
            Name = name;
            Description = description;
            CreationDate = DateTime.Now;
            Deadline = deadline;
            addToReport("Project created.");
        }

        /// <summary>
        /// Adds a task to the project.
        /// </summary>
        /// <param name="task">The task to add.</param>
        public void AddTask(Task task)
        {
            Tasks.Add(task);
            addToReport("Task added.");
        }

        /// <summary>
        /// Displays project details and tasks.
        /// </summary>
        public void DisplayDetails()
        {
            Console.WriteLine("Project: " + Name + ", Description: " + Description + ", Created: " + CreationDate + ", Deadline: " + Deadline + ", Status: " + Status);
            Console.WriteLine("Tasks:");
            foreach (var task in Tasks)
            {
                Console.WriteLine("- Task details not available yet");
            }
        }

        /// <summary>
        /// Generates a report of all actions performed on the project.
        /// </summary>
        public string GenerateReport()
        {
            return report;
        }
    }
}