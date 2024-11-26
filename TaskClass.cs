// Enum representing possible task states

public enum TaskStatus
{
	NotStarted,
	InProgress,
	Completed,
	OnHold       // If some issue prevents progression on a task
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

	public string TaskName { get; private set; }
	public string Description { get; private set; }
	public TaskStatus Status { get; private set; }
	public TaskPriority Priority { get; private set; }
	public DateTime CreatedOn { get; private set; } // Timestamping the creation of a task
	public DateTime? CompletedOn { get; private set; } // also the completion date
	public DateTime? DueDate { get; private set; }  // Optional parameter for a deadline
	public string IsAssignedTo { get; private set; }


	// Constructor for intializing new tasks

	public Task(string taskName, string description, TaskPriority priority, DateTime? dueDate = null) // default value set to null since this parameter is optional
	{
		if (string.IsNullOrEmpty(taskName)) // Validation for task name
			throw new ArgumentNullException("Task must have a name");

		// Properties initialized

		TaskName = taskName;
		Description = description;
		Priority = priority;
		Status = TaskStatus.NotStarted; // A new task hasn't been started as default
		CreatedOn = DateTime.Now; // To record creation time
		DueDate = dueDate;
	}

	// Method for changing task status

	public void ChangeTaskStatus(TaskStatus newStatus)
	{
		if (Status == TaskStatus.Completed) // Finished task's shouldn't be modified
			throw new InvalidOperationException("Status of a completed task should not be changed");

		Status = newStatus;

		if (newStatus == TaskStatus.Completed)
		{
			CompletedOn = DateTime.Now; // Timestamp set when task is completed
		}

	}

	// Method to assign a task to a user

	public void AssignToUser(string username)
	{
		if (string.IsNullOrWhiteSpace(username))
			throw new ArgumentException("The username should not be empty");

		IsAssignedTo = username;
	}

	// Boolean method for checking if further action is required for a task

	public bool ActionRequired()
	{
		return Status != TaskStatus.Completed; // Return will be true (action is required) if status isn't "Completed"
	}

	// Method to check if a task is overdue

	public bool IsTaskOverdue()
	{
		if (!DueDate.HasValue) return false; // If there's not due date, a task cannot be overdue

		// Checking if current date is past due date & task is not "Completed"
		return DateTime.Now > DueDate.Value && Status != TaskStatus.Completed;
	}

	// Method used to display all details of a task

	public void DisplayTaskDetails()
	{
		Console.WriteLine($"Task Name: {TaskName}");
		Console.WriteLine($"Description: {Description}");
        Console.WriteLine($"Priority: {Priority}");
        Console.WriteLine($"Status: {Status}");
        Console.WriteLine($"Created On: {CreatedOn}");

		// To display the task's assigned user or default to "Not assigned"
		Console.WriteLine($"Task Assigned To: {IsAssignedTo ?? "Not Assigned"}");

        // To display a due date if one is set
        if (DueDate.HasValue)
        {
            Console.WriteLine($"Task Due Date: {DueDate.Value}");
        }

		// Display completion time if task is finished
		if (CompletedOn.HasValue)
        {
            Console.WriteLine($"Task Completed On: {CompletedOn.Value}");
        }

        // To show when a task is overdue
        if (IsTaskOverdue())
        {
            Console.WriteLine("The task is currently overdue");
        }

    }

}






