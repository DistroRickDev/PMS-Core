using System;
using System.Collections.Generic;

namespace PMSCore {
    //class Task with attributes: Name, Description, Status; private attribute Status demonstrates encapsulation
    public class Task { 
        public string Name { get; set; } //naming the Task
        public string Description { get; set; } //providing a brief description of the Task
        public string Status { get; private set; } = "ToDo"; //initial Status is ToDo, it is protected (only accessible from within the class and derived classes)

        public Task(string name, string description) //constructor part, to initialize a new task, hiding internal components of the Task => abstraction   
        {
            Name = name; //setting the name for it
            Description = description; //providing a description
        }

        public void UpdateStatus(string newStatus) //method that updates a new value for this task => abstraction (status can't be changed directly)
        {
            if (newStatus == "ToDo" || newStatus == "In Progress" || newStatus == "Completed") //update status only if it matches one of these: "ToDo", "In Progress", "Completed"
                Status = newStatus;
        }
    }
    //class Project which is a group of tasks, also its attributes and methods are another example of Abstraction
    public class Project { 
        public string Name { get; set; } //attribute to give this project a name
        public string Description { get; set; } //providing the project's description
        public DateTime CreationDate { get; } = DateTime.Now; //creation date, according to the instructions that each project will have a timestamp of its creation
        public DateTime Deadline { get; set; } //instructions state that the project needs to have a deadline in addition to a name and description
        public List<Task> Tasks { get; } = new List<Task>(); //each project is supposed to have a list of tasks

        public Project(string name, string description, DateTime deadline)
        {
            Name = name; //setting the name for the project
            Description = description; //providing its description
            Deadline = deadline; //deciding the deadline
        }

        public void AddTask(Task task) => Tasks.Add(task); //adding a task to this project, this manages tasks => encapsulation, established the connection shown on the class diagram

        public void DisplayDetails() //displaying the project details, an example of abstraction
        {
            Console.WriteLine($"Project: {Name}, Description: {Description}, Created: {CreationDate}, Deadline: {Deadline}");
            //going through each task and displaying details 
            foreach (var task in Tasks)
            {
                Console.WriteLine($"Task: {task.Name}, Status: {task.Status}");
            }
        }
    }
}
