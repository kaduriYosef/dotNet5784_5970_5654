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

    //returns an integer that gets from the user
    static private int getInteger()
    {
        int input=0;
        while (!int.TryParse(Console.ReadLine(), out input)) ;
        return input ;
    }
    static void Main(string[] args)
    {
        try
        {
            Initialization.Do(s_dalTask, s_dalEngineer, s_dalDependency);
            int choice, sub_choice;
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
                sub_choice = getInteger();
                try
                {
                    
                    if( choice!=0)
                        SubMenu(choice,sub_choice);
   
                   
                }
                catch (Exception ex) { Console.WriteLine(ex); }
            }
            while (choice != 0);
            Console.WriteLine("bye bye.");
        }
        catch (Exception exc) { Console.WriteLine(exc); }

    }

    static private void SubMenu(int choice,int sub_choice)
    {
       while(sub_choice!=0)
        {
            switch(sub_choice)
            {
                case 1:
                    create(choice);
                    break;
                case 2:
                    read(choice);
                    break;
                case 3:
                    read_all(choice);
                    break;
                case 4:
                    update(choice);
                    break;  
                case 5:
                    delete(choice);
                    break;
                case 6: 
                    does_exist(choice);
                    break;
                    
                
            }
            sub_choice=getInteger();
        }
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

    private static void create(int choice)
    {

        Console.WriteLine("Enter all the variables needed.");
        switch(choice) 
        {
            case 1:
                break; 
            case 2:
                break;
            case 3:
                break;
        }

    }
    private static void read(int choice) { }
    private static void read_all(int choice) { }
    private static void update(int choice) { }
    private static void delete(int choice) { }
    private static void does_exist(int choice) { }
}
