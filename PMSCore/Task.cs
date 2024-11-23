// Enum representing possible task states

public enum TaskStatus
{
	New,
	InProgress,
	Completed,
	Blocked       // If some issue prevents progression on a task
}

// Enum representing priotity level of a task

public enum TaskPriority
{
	Low,
	Medium,
	High,
	Critical
}

public class Task
{
	// Properties for a task object

	public string TaskName { get; set; }
	public string Description { get; set; }
	public TaskStatus Status { get; set; }
	public TaskPriority Priority { get; set; }


	// Constructor for intializing new tasks

	public Task(string taskName, string description, TaskPriority priority, DateTime? dueDate = null) // default value set to null since this parameter is optional
	{
		if (string.IsNullOrEmpty(taskName)) // Validation for task name
        {	
			Console.WriteLine("Task must have a name");
			return;
		}

		if (string.IsNullOrEmpty(description))
        {
            Console.WriteLine("Task must have a description");
            return;
        }

        // Properties initialized

        TaskName = taskName;
		Description = description;
		Priority = priority;
		Status = TaskStatus.New; // A new task hasn't been started as default
	}

	// Method for changing task status

	public void ChangeTaskStatus(TaskStatus newStatus)
	{
		Status = newStatus;
	}


	// Boolean method for checking if further action is required for a task

	public bool ActionRequired()
	{
		return Status != TaskStatus.Completed; // Return will be true (action is required) if status isn't "Completed"
	}

	// Method used to display all details of a task

	public void DisplayTaskDetails()
	{
		Console.WriteLine($"Task Name: {TaskName}");
		Console.WriteLine($"Description: {Description}");
        Console.WriteLine($"Priority: {Priority}");
        Console.WriteLine($"Status: {Status}");

    }

}






