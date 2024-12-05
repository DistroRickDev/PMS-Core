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
        private string actionLog = "";

        /// <summary>
        /// Creates a project with a name, description, and deadline.
        /// </summary>
        public Project(string name, string description, DateTime deadline)
        {
            Name = name;
            Description = description;
            CreationDate = DateTime.Now;
            Deadline = deadline;
            actionLog += DateTime.Now + ": Project created.\n";
        }

        /// <summary>
        /// Adds a task to the project.
        /// </summary>
        /// <param name="task">The task to add.</param>
        public void AddTask(Task task)
        {
            Tasks.Add(task);
            actionLog += DateTime.Now + ": Task added.\n";
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
        public void GenerateReport()
        {
            Console.WriteLine("Action Log:");
            Console.WriteLine(actionLog);
        }
    }

    /// <summary>
    /// Represents a task to be defined later.
    /// </summary>
    public class Task { }
}