namespace BlTest;
using BO; // Assuming the namespace for the classes
using System.Xml;

class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void Main()
    {
        // Create an engineer with tasks
        Engineer engineer = new Engineer
        {
            Id = 1,
            Name = "John Doe",
            Email = "john.doe@example.com",
            Level = EngineerExperience.Expert,
            Cost = 5000,
            Task = new TaskInEngineer { Id = 101, Alias = "Initial Setup" },
            Tasks = new List<TaskInEngineer>
            {
                new TaskInEngineer { Id = 102, Alias = "Database Design" },
                new TaskInEngineer { Id = 103, Alias = "API Development" }
            }
        };
        List<TaskInList> tasks = new List<TaskInList>()
        {
            new TaskInList()
            {
                Id= 1,
                Alias="t1",
                Description=null,
                Status=BO.Status.Scheduled
            },
            new TaskInList()
            {
                Id= 1,
                Alias="t1",
                Description=null,
                Status=BO.Status.Scheduled
            },
            new TaskInList()
            {
                Id= 1,
                Alias="t1",
                Description=null,
                Status=BO.Status.Scheduled
            },
            new TaskInList()
            {
                Id= 1,
                Alias="t1",
                Description=null,
                Status=BO.Status.Scheduled
            }
        };
        Console.WriteLine(tasks.ToStringProperty());
        
        //foreach(var task in tasks) Console.WriteLine(task);
        //Console.WriteLine(engineer.ToString());

        return;
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();
    }
}
