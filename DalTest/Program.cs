
namespace DalTest;
using Dal;
using DalApi;
using DO;
using System;
using System.Collections.Specialized;

public class Program
{
    // Static instances for Dependency, Engineer, and Task services
    private static ITask? s_dalTask = new TaskImplementation();
    private static IEngineer? s_dalEngineer = new EngineerImplementation();
    private static IDependency? s_dalDependency = new DependencyImplementation();

    // Main method - the entry point of the application
    private static void Main(string[] args)
    {
        try
        {
            // Initialize the services with required dependencies
            Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency );

            // Get user input from the main menu
            int choice = 0;
            int id; // Variable to store IDs entered by the user

            // Main loop to handle user choices
            do
            {
                choice = menu(); // Display the main menu again
                switch (choice)
                {
                    case 0:
                        Console.WriteLine("bye bye");
                        break;
                    case 1: // Task operations
                        int OpForTask = options(); // Display Task options
                        while (OpForTask != 0)
                        {
                            switch (OpForTask)
                            {
                                case 1:
                                    createTask(); // Create a new Task
                                    break;
                                case 2:
                                    Console.WriteLine("Enter the Task's ID:");
                                    id = getInteger();
                                    PrintTheReadFunctionOfTask(s_dalTask!.Read(id)!); // Display a specific Task
                                    break;
                                case 3:
                                    PrintTheReadAllFunctionOfTask(s_dalTask!.ReadAll()); // Display all Tasks
                                    break;
                                case 4:
                                    s_dalTask!.Update(UpdateHelperTask()); // Update a Task
                                    break;
                                case 5:
                                    Console.WriteLine("Enter the ID of the Task you want to delete:");
                                    id = int.Parse(Console.ReadLine()!);
                                    s_dalTask!.Delete(id); // Delete a Task
                                    break;
                                case 0:
                                    // Exit operations for task
                                    break;
                            }
                            OpForTask = options(); // Show Task options again
                        }
                        break;
                    case 2: // Dependency operations
                        int OpForDependency = options(); // Display Dependency options
                        while (OpForDependency != 0)
                        {
                            switch (OpForDependency)
                            {
                                case 1:
                                    createDependency(); // Create a new Dependency
                                    break;
                                case 2:
                                    Console.WriteLine("Enter the Dependency's ID:");
                                    id = getInteger();
                                    // Display a specific Dependency
                                    PrintTheReadfunctionOfDependency(s_dalDependency!.Read(id)!); 
                                    break;
                                case 3:
                                    // Display all Dependencies
                                    PrintTheReadAllFunctionOfDependency(s_dalDependency!.ReadAll()); 
                                    break;
                                case 4:
                                    s_dalDependency!.Update(UpdateHelperForDepend()); // Update a Dependency
                                    break;
                                case 5:
                                    Console.WriteLine("Enter the id of the Dependency to delete:");
                                    id = getInteger();
                                    s_dalDependency!.Delete(id); // Delete a Dependency
                                    break;
                                case 6:
                                    Console.WriteLine("Enter the id of the dependent task");
                                    int dependent = getInteger();
                                    Console.WriteLine("enter the id of the task of which the first one depends on");
                                    int depnedsOn=getInteger();
                                    Console.WriteLine(s_dalDependency!.DoesExist(dependent,depnedsOn));
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
                                    id = int.Parse(Console.ReadLine()!);
                                    PrintTheReadfunctionOfEngineer(s_dalEngineer!.Read(id)!); // Display a specific Engineer
                                    break;
                                case 3:
                                    PrintTheReadAllFunctionOfEngineer(s_dalEngineer!.ReadAll()); // Display all Engineers
                                    break;
                                case 4:
                                    s_dalEngineer!.Update(UpdateHelperForEngineer()); // Update an Engineer
                                    break;
                                case 5:
                                    Console.WriteLine("Enter the ID of the Engineer you want to delete:");
                                    id = int.Parse(Console.ReadLine()!);
                                    s_dalEngineer!.Delete(id); // Delete an Engineer
                                    break;
                                default:
                                    OpForEngineer = 0; // Exit Engineer operations
                                    break;
                            }
                            OpForEngineer = options(); // Show Engineer options again
                        }
                        break;
                    default:
                        Console.WriteLine("Enter a Valid value"); // Handle invalid input
                        break;
                } // End of switch statement

            } while (choice != 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message); // Exception handling
        }
    }


    // Menu function to display the main menu and capture user's choice
    private static int menu()
    {
        printStringArray(new string[] {
        "choose from next list",
        "0 - exit",
        "1 - Task",
        "2 - Dependency",
        "3 - Engineer"
        });
        int firstmenu = getInteger();
       
        return firstmenu;
    }
    // Function to display a submenu for CRUD operations and capture user's choice
    private static int options(string[]? additional = null)
    {
        
        // showing CRUD operations
        printStringArray(new string[] 
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
            printStringArray(additional);
                
        int op = getInteger();
        
        return op;
    }
    private static int optionsForDepedency()
    {
        return options(new string[] {"6 - DoesExist"});
    }

    // Function to create a new Dependency
    private static void createDependency()
    {
       
        Console.WriteLine("Enter the id of the dependent Task");
        int dependentId = getInteger();
        Console.WriteLine("Enter the Task of which the first one depends on");
        int dependsOnTaskId = getInteger();
        Dependency dependency = new Dependency(0, dependentId, dependsOnTaskId);
        s_dalDependency!.Create(dependency);
    }

    // Function to create a new Engineer
    private static void createEngineer()
    {
        Console.WriteLine("Enter your ID:");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter your email:");
        string email = Console.ReadLine()!;
        Console.WriteLine("Enter your hourly cost:");
        double cost = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter your name:");
        string name = Console.ReadLine()!;
        Console.WriteLine("Enter your experience level (1-5)");
        //int l = int.Parse(Console.ReadLine()!);
        int l=0;
        l = getInteger();
        if (l > 5 || l < 0)
        {
            Console.WriteLine("ERROR\nEnter number again please");
            l = int.Parse(Console.ReadLine()!);
        }
        DO.EngineerExperience level = (EngineerExperience)l;
        Engineer engineer = new Engineer(id, email, cost, name, level, true);
        s_dalEngineer!.Create(engineer);
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
        EngineerExperience complexity = (EngineerExperience)getInteger();
        Console.WriteLine("Enter Deliverables");
        string deliverables = Console.ReadLine()!;
        Console.WriteLine("Enter any Remarks");
        string remarks = Console.ReadLine()!;
        Console.WriteLine("Enter the Engineer ID:");
        int engineerId = int.Parse(Console.ReadLine()!);
        Task task = new Task(0, alias, description, createdAtDate, null, isMilestone, complexity, null, null, null, null, deliverables, remarks, engineerId);
        s_dalTask!.Create(task);
    }
    private static Task UpdateHelperTask()//func to create item for Update Task
    {
        Console.WriteLine("Enter the id of the Task you wish to update:");
        int id = getInteger();
        Console.WriteLine("Enter an Alias");
        string alias = Console.ReadLine()!;
        Console.WriteLine("Enter a Description");
        string description = Console.ReadLine()!;
        DateTime createdAtDate = DateTime.Now;

        Console.WriteLine("Is this task a Milestone? (Y or N):");
        bool isMilestone = Console.ReadLine()!.ToLower() == "Y";

        Console.WriteLine("Enter Complexity Level (0 f- Beginner, 1 - AdvancedBeginner, etc.):");
        EngineerExperience complexity = (EngineerExperience)getInteger();
        Console.WriteLine("Enter Deliverables");
        string deliverables = Console.ReadLine()!;
        Console.WriteLine("Enter any Remarks");
        string remarks = Console.ReadLine()!;
        Console.WriteLine("Enter the Engineer ID:");
        int engineerId = int.Parse(Console.ReadLine()!);
        Task temp = new Task(id, alias, description, createdAtDate, null, 
            isMilestone, complexity, null, null, null, null, deliverables, remarks, engineerId);
        
        return temp;
    }
    private static Dependency UpdateHelperForDepend()//func to create item for Update Dependency
    {
        Console.WriteLine("Enter your ID");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter your ID of Depenency");
        int dependencyNow = Console.Read();
        Console.WriteLine("Enter your ID of most previose Depenency");
        int dependencyDep = int.Parse(Console.ReadLine()!);
        DO.Dependency temp = new Dependency(id, dependencyNow, dependencyDep);
        return temp;
    }
    private static Engineer UpdateHelperForEngineer()//func to create item for Update Engineer
    {
        Console.WriteLine("Enter your ID:");
        int id = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter your email:");
        string email = Console.ReadLine()!;
        Console.WriteLine("Enter your hourly cost:");
        double cost = int.Parse(Console.ReadLine()!);
        Console.WriteLine("Enter your name:");
        string name = Console.ReadLine()!;
        Console.WriteLine("Enter your experience level (1-5)");
        int l = int.Parse(Console.ReadLine()!);
        
        DO.EngineerExperience level = (EngineerExperience)l;
        DO.Engineer temp = new Engineer(id, email, cost, name, level, true);
        return temp;
    }
    // Prints detailed information of a single Task object.
    private static void PrintTheReadFunctionOfTask(Task ToPrint)
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
        Console.WriteLine(ToPrint.Copmlexity);
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
    private static void PrintTheReadfunctionOfDependency(Dependency ToPrint)
    {
        Console.Write("ID: ");
        Console.WriteLine(ToPrint.Id);
        Console.Write("Current Task: ");
        Console.WriteLine(ToPrint.DependentTask);
        Console.Write("The current Task depends on the task: ");
        Console.WriteLine(ToPrint.DependsOnTask + "\n");
    }
    // Outputs the information of a single Engineer object, including ID, email, cost, name, level, and active status.
    private static void PrintTheReadfunctionOfEngineer(Engineer ToPrint)
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
    private static void PrintTheReadAllFunctionOfTask(List<Task> toPrint)
    {
        foreach (DO.Task task in toPrint)
        {
            PrintTheReadFunctionOfTask(task);
        }
    }

    // Prints details of each Dependency object in the provided list
    private static void PrintTheReadAllFunctionOfDependency(List<Dependency> toPrint)
    {
        foreach (DO.Dependency dependency in toPrint)
        {
            PrintTheReadfunctionOfDependency(dependency);
        }
    }

    // Prints details of each Engineer object in the provided list
    private static void PrintTheReadAllFunctionOfEngineer(List<Engineer> toPrint)
    {
        foreach (DO.Engineer engineer in toPrint)
        {
            PrintTheReadfunctionOfEngineer(engineer);
        }
    }

    

    //returns an integer that gets from the user
    static private int getInteger()
    {
        int input = 0;
        while (!int.TryParse(Console.ReadLine(), out input)) ;
        return input;
    }
    static private void printStringArray(string[] arr) { foreach (string s in arr) Console.WriteLine(s); }
}

// namespace DalTest;

//using Dal;
//using DalApi;
//using System.ComponentModel.Design;
//using System.Runtime.InteropServices;

//internal class Program
//{
//    private static ITask? s_dalTask = new TaskImplementation(); //stage 1
//    private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
//    private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1

//    //returns an integer that gets from the user
//    static private int getInteger()
//    {
//        int input=0;
//        while (!int.TryParse(Console.ReadLine(), out input)) ;
//        return input ;
//    }
//    static void Main(string[] args)
//    {
//        try
//        {
//            Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency);
//            int choice, sub_choice;
//            do
//            {
//                printStringArray(new string[]{
//                    "Enter which object you want to choose",
//                    "1 - Task",
//                    "2 - Engineer",
//                    "3 - Dependency",
//                    "0 - exit"
//                });
//                choice = getInteger();
//                printMenu(choice);
//                sub_choice = getInteger();
//                try
//                {

//                    if( choice!=0)
//                        SubMenu(choice,sub_choice);


//                }
//                catch (Exception ex) { Console.WriteLine(ex); }
//            }
//            while (choice != 0);
//            Console.WriteLine("bye bye.");
//        }
//        catch (Exception exc) { Console.WriteLine(exc); }

//    }

//    static private void SubMenu(int choice,int sub_choice)
//    {
//       while(sub_choice!=0)
//        {
//            switch(sub_choice)
//            {
//                case 1:
//                    create(choice);
//                    break;
//                case 2:
//                    read(choice);
//                    break;
//                case 3:
//                    read_all(choice);
//                    break;
//                case 4:
//                    update(choice);
//                    break;  
//                case 5:
//                    delete(choice);
//                    break;
//                case 6: 
//                    does_exist(choice);
//                    break;


//            }
//            sub_choice=getInteger();
//        }
//    }

//   
//    /// <summary>
//    /// Displays a menu based on the specified choice, providing options related to tasks, engineers, and dependencies.
//    /// </summary>
//    /// <param name="choice">The user's selection indicating the type of menu to display (1 for Task, 2 for Engineer, 3 for Dependency).</param>
//    static private void printMenu(int choice)
//    {
//        // Determine the type based on the user's choice
//        string type = "";
//        switch (choice)
//        {
//            case 1: type = "Task"; break;
//            case 2: type = "Engineer"; break;
//            case 3: type = "Dependency"; break;
//        }

//        // Display menu options based on the selected type
//        printStringArray(new string[]
//        {
//        "Enter what do you wish to do",
//        "0 - Go back",
//        "1 - Create new " + type,
//        "2 - Read an existing " + type,
//        "3 - Read every " + type,
//        "4 - Update an existing " + type,
//        "5 - Delete an existing " + type
//        });

//        // Additional option for Dependency type
//        if (choice == 3)
//            printStringArray(new string[]
//            {
//            "6 - Show if a Dependency exists"
//            });
//    }

//    private static void create(int choice)
//    {

//        Console.WriteLine("Enter all the variables needed.");
//        switch(choice) 
//        {
//            case 1:
//                break; 
//            case 2:
//                break;
//            case 3:
//                break;
//        }

//    }
//    private static void read(int choice) 
//    {

//        Console.WriteLine("Enter all the needed parameters.");
//        switch(choice)
//        {
//            case 1:
//                string alias=Console.ReadLine()!;
//                break;
//            case 2:
//                break;
//            case 3:
//                break;
//        }
//    }
//    private static void read_all(int choice) { }
//    private static void update(int choice) { }
//    private static void delete(int choice) { }
//    private static void does_exist(int choice) { }
//}
