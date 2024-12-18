## Team 6: Project Management System
#### V0.8

------------

## Table of Contents

- Introduction
- Core Architecture
- UML Diagrams
- Classes
- Implementation Decisions
- Deployment Instructions

## Introduction
In this project, the objective is to collaboratively design and implement a comprehensive Project Management System using Polymorphism, Inheritance, Abstraction, and Encapsulation.

###  Objectives

The system aims to streamline projects, task allocation, and reporting, fostering better collaboration and productivity with a flexible system capable of expansion. By utilizing Inheritance, the system ensures reusability and hierarchical relationships between classes, while Abstraction focuses on simplifying the complexity of operations for the end-user.

Encapsulation safeguards the integrity of the data, maintaining security and coherence across tasks. Lastly, Polymorphism enables the system to adapt seamlessly to diverse project requirements and use cases.

###  Team Goal

This project not only emphasizes technical proficiency but also promotes teamwork, problem-solving, and innovation on Team 6, as we aim to deliver a practical tool for real-world applications with this project.

## Core Architecture

The core of the Project Task Management System revolves around the application of Object-Oriented Programming (OOP) principles to design a modular, flexible, and scalable architecture. The team began with a core outline in how to tease out the rest of the process such as seen in the below image:

![](https://webteic.info/images/PMScore-1.jpg)
> Screenshot: Core Outline

This core architecture highlights the carefully structured relationships, ensuring seamless task and project management functionality. The MVP contains the PMS Core where all key actions related to Project Management are contained, while other areas are connected by API calls for control & flexibility. 

The PMS Client is where we contain the users of the system & interface for interactions, while PMS Crypto is another component to help with encrypting user passwords that is built into ASP.Net called Cross-Platform Cryptography. We also used one of the utilities of the framework called gRPC.

To maintain persistent storing of information the team decided to have a Json structure of a client/server relationship. This is very good for parsing and serializing data for the project with C#. These are micro-services and the overall Core Architecture planned.

###  Encapsulation

Each class is designed to encapsulate its attributes and behaviors, such as a Task class containing properties like name, priority, and status, along with methods for updating progress or assigning users. This encapsulation ensures data integrity and controlled access through setters and getters.

###  Inheritance

A hierarchical design allows shared functionalities to propagate across related entities. For example, a User class serves as a superclass (parent), with specialized subclasses like Admin, ProductOwner, Developer and Tester inheriting core methods such as register / login authentication while introducing unique functionalities like user management and task assignment.

###  Polymorphism

The system supports dynamic behavior through polymorphism, enabling multiple forms of actions under a common interface. For instance, a generateReport() method can be implemented differently across classes to produce outputs, such as individual task progress or overall project status.

###  Abstraction

High-level interfaces abstract the underlying complexities of task management.

Database Integration: The system employs a json setup (database) to store and manage critical information. Relationships like one-to-many (e.g., one project containing many tasks) are mapped effectively using OOP concepts, ensuring consistent and logical data representation.

Scalability and Modularity: With an emphasis on modular design, the system is scalable, allowing new features to be integrated without disrupting existing functionality.

By combining these OOP principles with efficient planning choices, the system ensures robust performance, maintainability, and adaptability to varied project management needs.

## UML Diagrams

During the planning phase the team worked on various UML diagrams such as the following:

![](https://webteic.info/images/PMScore-2.jpg)
> Screenshot: UML Diagram 1

------------

Researching the needs & objectives further, the team broke this down into key highlights:

- A system capable of registering different types of  Users of different types  and assign role-specific permissions
- A system capable of creating different types of projects
- A system capable of creating different Project Items
- A system capable of managing different  task states and track any progress
- A system capable of managing different states of the project
- A system capable of generating reports on tasks
- A system  capable of allowing users to track their progress
- A system capable of generating project reports
- A system capable of retaining data over cycles
- A system capable of allocating and deallocating users to projects and tasks
- A system capable of Role-based access

This led to enhancing the UML Diagrams. 

![](https://webteic.info/images/PMScore-3.jpg)
> Screenshot: UML Diagram 2 - redefined

------------

![](https://webteic.info/images/PMScore-4.jpg)
> Screenshot: UML Diagram 3 - working out next steps in details

------------

![](https://webteic.info/images/PMScore-5.jpg)
> Screenshot: UML Diagram 4 - further redefinement

------------

The following is the UML diagram settled on that the development could proceed.

![](https://webteic.info/images/PMScore-6.jpg)
> Screenshot: UML Diagram for the project agreed & development begins.

------------

During the development of the project, the UML Diagrams were updated to the following:

![](https://webteic.info/images/PMScore-6-1.jpg)
> Screenshot: UML Diagram enhancing the workflow with TaskManager.

![](https://webteic.info/images/PMScore-6-2.jpg)
> Screenshot: UML Diagram enhancing the workflow with ProjectManager.

![](https://webteic.info/images/PMScore-6-3.jpg)
> Screenshot: UML Diagram enhancing the workflow with UserManager.

## Classes

The following outlines the key classes involved in the project:


| Class: ProjectClass.cs | Type: Encapsulation & abstraction |
|:----------------|:----------------------|
| *Details* | **Encapsulation:** The report field and the addToReport method are private, ensuring controlled access to the internal data.<br/><br/> **Abstraction:** AddTask, DisplayDetails, and GenerateReport methods hide how tasks and reports are managed, focusing on functionality. |

| Class: ReportManager.cs | Type: Encapsulation, abstraction & polymorphism |
|:----------------|:----------------------|
| *Details* | **Encapsulation:** The private fields _instance and _logger don't allow direct access, methods GetInstance and GenerateProjectReport are used to communicate with this class. <br/><br/> **Abstraction:** The GenerateProjectReport method simplifies interactions, hiding the complexities of logging and report generation. <br/><br/> **Polymorphism:** This class uses dependency injection with ILogger, which lets the class use different types of loggers. |

| Class: Task.cs | Type: Encapsulation, abstraction & inheritance |
|:----------------|:----------------------|
| *Details* | **Encapsulation:** The properties ensure that task data is accessed and modified securely. <br/><br/> **Abstraction:** The class hides unnecessary implementation details. <br/><br/> **Inheritance:** This class is used as a base class for more specialized types of tasks. |

| Class: Worker.cs | Type: Encapsulation, abstraction, inheritance & polymorphism |
|:----------------|:----------------------|
| *Details* | **Encapsulation:** The _logger field is private, preventing direct access and ensuring secure handling of logging. <br/><br/> **Abstraction:** The ExecuteAsync method simplifies background task execution by hiding the underlying implementation details. <br/><br/> **Inheritance:** The class inherits from BackgroundService, using its framework to handle background tasks. <br/><br/> **Polymorphism:** This class overrides the ExecuteAsync method to define specific behavior for this worker while maintaining the BackgroundService structure. |

| Class: ProjectOwner.cs | Type: Encapsulation, abstraction, inheritance & polymorphism |
|:----------------|:----------------------|
| *Details* | **Encapsulation:** Sensitive information is kept private and can only be accessed through methods. <br/><br/> **Abstraction:** The shared actions, such as managing permissions, are defined in an abstract class. <br/><br/> **Inheritance:** ProjectOwner inherits from its base class and shares the same properties with other types of users. <br/><br/> **Polymorphism:** The AssignTask method is overridden to implement behavior customized to project-specific needs.|

## Implementation Decisions

The team worked with the GitHub task management system for managing the workload, pipelines and commits.

![](https://webteic.info/images/PMScore-7.jpg)
> Screenshot: Team development workload.

The tasks were agreed with each person on the team & setup of the kanban board proceeded with column limits to ensure a smooth process.

During the development we would do unit tests at the same time of developing each task, a continuous cycle to reduce the number of potential bugs that could appear later on.

Branches would be used with GitHub for each persons changes before being merged later into the master.

The team encountered a major bottleneck later in the project lifecycle and the original scope needed to be reshaped. During a few team meetings, we decided to reduce the number of tasks and combine certain processes. This would enhance the project while also making more attainable to achieve the goals set out. 

UML Diagrams were updated to account for this with the User -> TaskManager, ProjectManager & UserManager -> with the improved StateManager. This streamlined the structure of the project and was one of the most vital implementation decisions made.

The bottlenecks were also removed with the team effort on additional reviewing & fixing, as well removing unnecessary tasks no longer needed with the improved structure. 

![](https://webteic.info/images/PMScore-7-1.jpg)
> Screenshot: Team development workload improvements.

During team meetings, everyone had their voice to address any issues and the team began to gel as one unit to reach the goals set out. Each person showed their own leadership abilities, support & working as one to deliver the project.

###  Testing

Rigorous testing (unit tests) was done from the outset with each task & classes. While this did slow down the overall development, it was better this way to allow a cleaner codebase that was not riddled with bugs.

The code base was enhanced with additional comments to layout a clear structure for each file & class.

![](https://webteic.info/images/PMScore-7-2.jpg)
> Screenshot: Unit Testing structure.

Various issues were found during the tests and were corrected, including refinements along the way. When a pull request was sent, another team member would jump in to review this pull request as a rule until the task & testing was complete.

## Deployment Instructions

Details here