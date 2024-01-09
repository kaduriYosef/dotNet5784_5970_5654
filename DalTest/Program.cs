 namespace DalTest;

using Dal;
using DalApi;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;

internal class Program
{
    private static ITask? s_dalTask = new TaskImplementation(); //stage 1
    private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
    private static IDependency? s_dalDependency = new DependencyImplementation(); //stage 1
    static void Main(string[] args)
    {
        try
        {
            Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency);
            int choice, subChoice;
            do
            {
                printStringArray(new string[]{
                    "Enter which object you want to choose",
                    "1 - Task",
                    "2 - Engineer",
                    "3 - Dependency",
                    "0 - exit"
                });
                choice = getInteger();
                printMenu(choice);
                subChoice = getInteger();
                try
                {
                    switch (choice)
                    {
                        case 0: Console.WriteLine("bye bye."); break;
                        case 1: TaskMenu(subChoice); break;
                        case 2: EngineerMenu(subChoice); break;
                        case 3: DependencyMenu(subChoice); break;
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
            while (choice != 0);
        }
        catch (Exception exc) { Console.WriteLine(exc); }

    }
    static private void  printStringArray(string[] arr) {foreach (string s in arr) Console.WriteLine(s);}
    /// <summary>
    /// Displays a menu based on the specified choice, providing options related to tasks, engineers, and dependencies.
    /// </summary>
    /// <param name="choice">The user's selection indicating the type of menu to display (1 for Task, 2 for Engineer, 3 for Dependency).</param>
    static private void printMenu(int choice)
    {
        // Determine the type based on the user's choice
        string type = "";
        switch (choice)
        {
            case 1: type = "Task"; break;
            case 2: type = "Engineer"; break;
            case 3: type = "Dependency"; break;
        }

        // Display menu options based on the selected type
        printStringArray(new string[]
        {
        "Enter what do you wish to do",
        "0 - Go back",
        "1 - Create new " + type,
        "2 - Read an existing " + type,
        "3 - Read every " + type,
        "4 - Update an existing " + type,
        "5 - Delete an existing " + type
        });

        // Additional option for Dependency type
        if (choice == 3)
            printStringArray(new string[]
            {
            "6 - Show if a Dependency exists"
            });
    }

    //returns an integer that gets from the user
    private static int getInteger()
    {
        int input=0;
        while (!int.TryParse(Console.ReadLine(), out input)) ;
        return input ;
    }
}
