﻿
namespace DalTest;
using Dal;
using DalApi;
using DO;
using System;
using System.Collections.Specialized;

public class Program
{
    //
    //static readonly IDal s_dal = new DalList(); //stage 2
    static readonly IDal s_dal = new DalXml(); //stage 3

    // Main method - the entry point of the application
    private static void Main(string[] args)
    {
        try
        {
            // Initialize the services with required dependencies


            // Get user input from the main menu
            int choice = 0;
            int id; // Variable to store IDs entered by the user

            // Main loop to handle user choices
            do
            {
                try
                {
                    choice = Menu(); // Display the main menu again
                    switch (choice)
                    {
                        case 0:
                            Console.WriteLine("bye bye");
                            break;
                        case 1: // Task operations

                            int opChoiceTask = options(); // Display Task options
                            while (opChoiceTask != 0)
                            {
                                switch (opChoiceTask)
                                {
                                    case 1:
                                        createTask(); // Create a new Task
                                        break;
                                    case 2:
                                        Console.WriteLine("Enter the Task's ID:");
                                        id = GetInteger();
                                        PrintSingleTask(s_dal!.Task.Read(id)!); // Display a specific Task
                                        break;
                                    case 3:
                                        PrintAllTasks(s_dal!.Task.ReadAll().ToList()!); // Display all Tasks
                                        break;
                                    case 4:
                                        s_dal!.Task.Update(TaskUpdateHelp()); // Update a Task
                                        break;
                                    case 5:
                                        Console.WriteLine("Enter the ID of the Task you want to delete:");
                                        id = GetInteger();
                                        s_dal!.Task.Delete(id); // Delete a Task
                                        break;
                                    case 0:
                                        // Exit operations for task
                                        break;
                                }
                                opChoiceTask = options(); // Show Task options again
                            }
                            break;
                        case 2: // Dependency operations
                            int OpForDependency = optionsForDependency(); // Display Dependency options
                            while (OpForDependency != 0)
                            {
                                switch (OpForDependency)
                                {
                                    case 1:
                                        createDependency(); // Create a new Dependency
                                        break;
                                    case 2:
                                        Console.WriteLine("Enter the Dependency's ID:");
                                        id = GetInteger();
                                        // Display a specific Dependency
                                        PrintSingleDependency(s_dal!.Dependency.Read(id)!);
                                        break;
                                    case 3:
                                        // Display all Dependencies
                                        PrintAllDependencies(s_dal!.Dependency.ReadAll().ToList()!);
                                        break;
                                    case 4:
                                        s_dal!.Dependency.Update(DepUpdateHelp()); // Update a Dependency
                                        break;
                                    case 5:
                                        Console.WriteLine("Enter the id of the Dependency to delete:");
                                        id = GetInteger();
                                        s_dal!.Dependency.Delete(id); // Delete a Dependency
                                        break;
                                    case 6:
                                        Console.WriteLine("Enter the id of the dependent task");
                                        int dependent = GetInteger();
                                        Console.WriteLine("enter the id of the task of which the first one depends on");
                                        int depnedsOn = GetInteger();
                                        Console.WriteLine(s_dal!.Dependency.DoesExist(dependent, depnedsOn) ? "Such dependency exist" : "No such dependency exist.");
                                        break;
                                    case 0:
                                        OpForDependency = 0; // Exit Dependency operations
                                        break;
                                }
                                OpForDependency = options(); // Show Dependency options again
                            }
                            break;
                        case 3: // Engineer operations
                            int OpForEngineer = options(); // Display Engineer options
                            while (OpForEngineer != 0)
                            {
                                switch (OpForEngineer)
                                {
                                    case 1:
                                        createEngineer(); // Create a new Engineer
                                        break;
                                    case 2:
                                        Console.WriteLine("Enter the engineer's ID:");
                                        id = GetInteger();
                                        PrintSingleEngineer(s_dal!.Engineer.Read(id)!); // Display a specific Engineer
                                        break;
                                    case 3:
                                        PrintAllEngineers(s_dal!.Engineer.ReadAll().ToList()!); // Display all Engineers
                                        break;
                                    case 4:
                                        s_dal!.Engineer.Update(EngUpdateHelp()); // Update an Engineer
                                        break;
                                    case 5:
                                        Console.WriteLine("Enter the ID of the Engineer you want to delete:");
                                        id = GetInteger();
                                        s_dal!.Engineer.Delete(id); // Delete an Engineer
                                        break;
                                    default:
                                        OpForEngineer = 0; // Exit Engineer operations
                                        break;
                                }
                                OpForEngineer = options(); // Show Engineer options again
                            }
                            break;
                        case 4: //reset
                            Console.Write("Would you like to create Initial data? (Y/N)"); //stage 3
                            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input"); //stage 3
                            if (ans == "Y") //stage 3
                                Initialization.Do(s_dal);
                            break;
                        default:
                            Console.WriteLine("Enter a Valid value"); // Handle invalid input
                            break;

                    }
                    Console.Write('\n');
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

            } while (choice != 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message); // Exception handling
        }
    }


    // Menu function to display the main menu and capture user's choice
    private static int Menu()
    {
        PrintStringArray(new string[] {
        "choose from next list",
        "0 - exit",
        "1 - Task",
        "2 - Dependency",
        "3 - Engineer",
        "4 - Init"
        });
        int firstmenu = GetInteger();

        return firstmenu;
    }
    // Function to display a submenu for CRUD operations and capture user's choice

    private static int options(string[]? additional = null)
    {

        // showing CRUD operations
        PrintStringArray(new string[]
        {
        "Your Options",
        "0 - Go back",
        "1 - Create",
        "2 - Read",
        "3 - Read All",
        "4 - Update",
        "5 - Delete"
        });
        if (additional != null)
            PrintStringArray(additional);

        int op = GetInteger();

        return op;
    }

    private static int optionsForDependency()
    {
        return options(new string[] { "6 - DoesExist" });
    }


    // Function to create a new Task
    private static void createTask()
    {
        Console.WriteLine("Enter an Alias");
        string alias = Console.ReadLine()!;
        Console.WriteLine("Enter a Description");
        string description = Console.ReadLine()!;
        DateTime createdAtDate = DateTime.Now;

        Console.WriteLine("should this task be a Milestone? (Y or N):");
        bool isMilestone = Console.ReadLine()!.ToLower() == "Y";

        Console.WriteLine("Enter Complexity Level (0 - Beginner, 1 - AdvancedBeginner, etc.):");
        EngineerExperience complexity = (EngineerExperience)GetInteger();

        Console.WriteLine("Enter Deliverables");
        string deliverables = Console.ReadLine()!;

        Console.WriteLine("Enter any Remarks");
        string remarks = Console.ReadLine()!;

        Console.WriteLine("Enter the Engineer ID:");
        int engineerId = GetInteger();

        Task task = new
        (
            Id: 0,
            Alias: alias,
            Description: description,
            CreatedAtDate: createdAtDate,
            IsMilestone: isMilestone,
            Complexity: complexity,
            Deliverables: deliverables,
            Remarks: remarks,
            EngineerId: engineerId
        );

        s_dal!.Task.Create(task);
    }

    // Function to create a new Dependency
    private static void createDependency()
    {

        Console.WriteLine("Enter the id of the dependent Task");
        int dependentId = GetInteger();
        Console.WriteLine("Enter the Task of which the first one depends on");
        int dependsOnTaskId = GetInteger();
        Dependency dependency = new(
            Id: 0,
            DependentTask: dependentId,
            DependsOnTask: dependsOnTaskId
            );

        s_dal!.Dependency.Create(dependency);
    }

    // Function to create a new Engineer
    private static void createEngineer()
    {
        Console.WriteLine("Enter the ID:");
        int id = GetInteger();

        Console.WriteLine("Enter the email:");
        string email = Console.ReadLine()!;

        Console.WriteLine("Enter the hourly cost:");
        double cost = GetInteger();

        Console.WriteLine("Enter the name:");
        string name = Console.ReadLine()!;

        Console.WriteLine("Enter the experience level 0 - 4.");
        int intLevel = GetInteger() % 5;
        EngineerExperience level = (EngineerExperience)intLevel;

        Engineer engineer = new Engineer(id, email, cost, name, level, Active: true);

        s_dal!.Engineer.Create(engineer);
    }


    private static Task TaskUpdateHelp()
    //function to create item for Update Task
    {
        Console.WriteLine("Enter the id of the Task you wish to update:");
        int id = GetInteger();
        
        Console.WriteLine("Enter an Alias");
        string alias = Console.ReadLine()!;
        
        Console.WriteLine("Enter a Description");
        string description = Console.ReadLine()!;
        DateTime createdAtDate = DateTime.Now;

        Console.WriteLine("Is this task a Milestone? (Y or N):");
        bool isMilestone = Console.ReadLine()!.ToLower() == "Y";

        Console.WriteLine("Enter Complexity Level (0 - 4):");
        EngineerExperience complexity = (EngineerExperience)GetInteger();
        
        Console.WriteLine("Enter Deliverables");
        string deliverables = Console.ReadLine()!;
        
        Console.WriteLine("Enter any Remarks");
        string remarks = Console.ReadLine()!;
        
        Console.WriteLine("Enter the Engineer ID:");
        int engineerId = GetInteger();
        
        Task temp = new 
            (
            Id: id,
            Alias: alias,
            Description: description,
            CreatedAtDate: createdAtDate,
            RequiredEffortTime: null,
            IsMilestone: isMilestone, 
            Complexity: complexity,
            StartDate: null,
            ScheduledDate: null,
            DeadlineDate: null,
            CompleteDate: null,
            Deliverables: deliverables,
            Remarks: remarks,
            EngineerId: engineerId
            );

        return temp;
    }

    private static Dependency DepUpdateHelp()//func to create item for Update Dependency
    {
        Console.WriteLine("Enter the ID");
        int id = GetInteger();
        
        Console.WriteLine("Enter the Id of the depndent task");
        int dependentT = GetInteger();
        
        Console.WriteLine("Enter the Id of the task for which the first one depends on");
        int dependsOnT = GetInteger();
        
        Dependency temp = new
            (Id: id,
            DependentTask: dependentT,
            DependsOnTask: dependsOnT
            );
        return temp;
    }

    private static Engineer EngUpdateHelp()//func to create item for Update Engineer
    {
        Console.WriteLine("Enter the ID:");
        int id = GetInteger();

        Console.WriteLine("Enter the email:");
        string email = Console.ReadLine()!;

        Console.WriteLine("Enter the hourly cost:");
        double cost = GetInteger();

        Console.WriteLine("Enter the name:");
        string name = Console.ReadLine()!;

        Console.WriteLine("Enter the experience level (0-4)");
        int l = GetInteger();

        DO.EngineerExperience level = (EngineerExperience)l;
        DO.Engineer temp = new(id, email, cost, name, level,Active: true);
        return temp;
    }

    // Prints detailed information of a single Task object.
    private static void PrintSingleTask(Task ToPrint)
    {
        Console.Write("ID: ");
        Console.WriteLine(ToPrint.Id);
        Console.Write("Alias: ");
        Console.WriteLine(ToPrint.Alias);
        Console.Write("Description: ");
        Console.WriteLine(ToPrint.Description);
        Console.Write("Created At Date: ");
        Console.WriteLine(ToPrint.CreatedAtDate);
        Console.Write("Required Effort Time: ");
        Console.WriteLine(ToPrint.RequiredEffortTime);
        Console.Write("Is Milestone: ");
        Console.WriteLine(ToPrint.IsMilestone);
        Console.Write("Copmlexity: ");
        Console.WriteLine(ToPrint.Complexity);
        Console.Write("Start Date: ");
        Console.WriteLine(ToPrint.StartDate);
        Console.Write("Scheduled Date: ");
        Console.WriteLine(ToPrint.ScheduledDate);
        Console.Write("Deadline Date: ");
        Console.WriteLine(ToPrint.DeadlineDate);
        Console.Write("Complete Date: ");
        Console.WriteLine(ToPrint.CompleteDate);
        Console.Write("Deliverables: ");
        Console.WriteLine(ToPrint.Deliverables);
        Console.Write("Remarks: ");
        Console.WriteLine(ToPrint.Remarks);
        Console.Write("Engineer Id: ");
        Console.WriteLine(ToPrint.EngineerId + "\n");
    }
    
    // Displays details of a single Dependency object, including its current and dependent tasks.
    private static void PrintSingleDependency(Dependency ToPrint)
    {
        Console.Write("ID: ");
        Console.WriteLine(ToPrint.Id);
        Console.Write("Current Task: ");
        Console.WriteLine(ToPrint.DependentTask);
        Console.Write("The current Task depends on the task: ");
        Console.WriteLine(ToPrint.DependsOnTask + "\n");
    }
    
    // Outputs the information of a single Engineer object, including ID, email, cost, name, level, and active status.
    private static void PrintSingleEngineer(Engineer ToPrint)
    {
        Console.Write("ID: ");
        Console.WriteLine(ToPrint.Id);
        Console.Write("Email: ");
        Console.WriteLine(ToPrint.Email);
        Console.Write("Cost: ");
        Console.WriteLine(ToPrint.Cost);
        Console.Write("Name: ");
        Console.WriteLine(ToPrint.Name);
        Console.Write("level: ");
        Console.WriteLine(ToPrint.Level);
        Console.Write("Active: ");
        Console.WriteLine(ToPrint.Active + "\n");
    }


    // Prints details of each Task object in the provided list
    private static void PrintAllTasks(List<Task> toPrint)
    {
        foreach (Task task in toPrint)
        {
            PrintSingleTask(task);
        }
    }

    // Prints details of each Dependency object in the provided list
    private static void PrintAllDependencies(List<Dependency> toPrint)
    {
        foreach (DO.Dependency dependency in toPrint)
        {
            PrintSingleDependency(dependency);
        }
    }

    // Prints details of each Engineer object in the provided list
    private static void PrintAllEngineers(List<Engineer> toPrint)
    {
        foreach (DO.Engineer engineer in toPrint)
        {
            PrintSingleEngineer(engineer);
        }
    }



    //returns an integer that gets from the user
    static private int GetInteger()
    {
        int input = 0;
        while (!int.TryParse(Console.ReadLine(), out input))
            Console.WriteLine("Expecting only integer now!");
        return input;
    }
    
    static private void PrintStringArray(string[] arr) { foreach (string s in arr) Console.WriteLine(s); }
}




