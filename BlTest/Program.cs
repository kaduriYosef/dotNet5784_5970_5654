using DalTest;

namespace BlTest;

using System.Reflection.Emit;
using BO; // Assuming the namespace for the classes

class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


    void Main(string[] args)
    {


        Console.Write("Would you like to create Initial data? (Y/N) ");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y" || ans == "y") DalTest.Initialization.Do();
        try
        {
            int chose = 0;
            do
            {
                chose = menuChoise();

                switch (chose)
                {
                    case 0:
                        break;
                    case 1:     //Engineer's entity
                        try
                        {
                            int choice1 = subMenuChoise("Engineer"); // showing CRUD operations
                            choiceInEntity(choice1, 1);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case 2:     //Task entity
                        try
                        {
                            int choice2 = subMenuChoise("Task"); // showing CRUD operations
                            choiceInEntity(choice2, 2);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    default:
                        Console.WriteLine("ERROR: choose number between 1-3");
                        chose = GetInteger();
                        break;
                }
                Console.WriteLine();
            } while (chose != 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }
    int menuChoise()
    {
        int choice;
        Console.WriteLine("Press - 0 exit");
        Console.WriteLine("Press - 1 for Engineer's entity");
        Console.WriteLine("Press - 2 for Task's entity");
        choice = GetInteger();
        return choice;
    }
    int subMenuChoise(string type)
    {
        Console.WriteLine("Please press which action you want to take:");
        Console.WriteLine("0 - Go back");
        Console.WriteLine($"1 - Create {type}");
        Console.WriteLine($"2 - Get an {type} by ID");
        Console.WriteLine($"3 - Get all {type}");
        Console.WriteLine($"4 - Update {type} data");
        Console.WriteLine($"5 - Delete {type}");
        if (type == "Task")
        {
            Console.WriteLine($"6 - Update Task date");
        }

        return GetInteger();
    }
    int choiceInEntity(int userChoice, int EngTask)
    {
        do
        {
            switch (userChoice)
            {
                case 0:     //exit
                    break;
                case 1: //create
                    if (EngTask == 1)
                        try { s_bl.Engineer.Create(GetEngineer()); }
                        catch (Exception ex) { Console.WriteLine(ex.Message); } //if ID is allredy exist
                    else if (EngTask == 2)
                        s_bl.Task.Create(GetTask());
                    break;
                case 2: //read
                    if (EngTask == 1)
                    {
                        Console.Write("Enter Engineer's ID: ");
                        Console.WriteLine(s_bl.Engineer.Read(GetInteger()));
                    }
                    else if (EngTask == 2)
                    {
                        Console.Write("Enter Task's ID: ");
                        Console.WriteLine(s_bl.Task.Read(GetInteger()));
                    }
                    break;
                case 3: //read all
                    if (EngTask == 1)
                        foreach (var item1 in s_bl.Engineer.ReadAll())
                        {
                            Console.WriteLine(item1);
                            Console.WriteLine();
                        }
                    else if (EngTask == 2)
                        foreach (var item2 in s_bl.Task.ReadAll())
                        {
                            Console.WriteLine($"Task ID: {item2}");
                            //Console.WriteLine($"Description: {item2.Description}");
                            //Console.WriteLine($"Alias: {item2.Alias}");
                            //Console.WriteLine($"Status: {item2.Status}");
                        }
                    break;
                case 4: //update
                    try
                    {
                        if (EngTask == 1)
                            s_bl.Engineer.Update(GetEngineer());
                        else if (EngTask == 2)
                            s_bl.Task.Update(GetTask());
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                    break;
                case 5: //delete
                    try
                    {
                        if (EngTask == 1)
                        {
                            Console.Write("Enter Engineer's ID: ");
                            s_bl.Engineer.Delete(GetInteger());
                        }
                        else if (EngTask == 2)
                        {
                            Console.Write("Enter Task's ID: ");
                            s_bl.Task.Delete(GetInteger());
                        }
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                    break;
                case 6://update date

                    Console.Write("Enter Task's ID: ");
                    int id = GetInteger();
                    Console.Write("Enter Task's Scheduled date: ");
                    DateTime scheduledDate = GetDateTime(); // check date
                  
                    s_bl.Task.UpdateDate(id, scheduledDate);
                    break;

                default:  //if the user choose wrong number 
                    Console.WriteLine("ERORR: choose numbers betwin 1-6");
                    userChoice = GetInteger();
                    break;
            }
        } while (userChoice < 0 || userChoice > 5);
        return 1;
    }
    BO.Engineer GetEngineer()
    {
        Console.WriteLine("Enter Engineer's details:");
        Console.Write("ID: ");
        int id = GetInteger();
        Console.Write("name: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";
        Console.Write("cost: ");
        int cost = GetInteger();
        Console.Write("level, Rating between 1-5: ");
        BO.EngineerExperience level = (BO.EngineerExperience)(checkNum());
        Console.WriteLine("Engineer's task:");
        Console.Write("Task's ID: "); int taskId = GetInteger();
        Console.Write("Task's alias: "); string taskAlias = Console.ReadLine() ?? "";
        BO.TaskInEngineer taskInEngineer = new BO.TaskInEngineer { Id = taskId, Alias = taskAlias };

        
        return new BO.Engineer
        {
            Id = id,
            Name = name,
            Email = email,
            Cost = cost,
            Level = level,
            Task = taskInEngineer
        };
    }
    BO.Task GetTask()
    {
        Console.WriteLine("Enter Task details:");
        Console.Write("ID: ");
        int id = GetInteger();

        Console.Write("Alias: ");
        string alias = Console.ReadLine() ?? "";

        Console.Write("Description: ");
        string description = Console.ReadLine() ?? "";

        Console.Write("Status, Rating between 1-5: ");
        BO.Status status = (BO.Status)(checkNum());

        Console.WriteLine("press 1 to add dependecy or 0 to skip:");
        int? check = GetInteger();
        List<BO.TaskInList>? dependency = new List<BO.TaskInList>();
        while (check == 1) //get all the parameter for all the dependcies of this task 
        {
            Console.Write("ID: "); int idTask = GetInteger();
            Console.Write("Description: "); string descruptionTask = Console.ReadLine();
            Console.Write("Alias: "); string aliasTask = Console.ReadLine();
            Console.Write("Status: "); BO.Status statusTask = (BO.Status)(checkNum());
            dependency.Add(new BO.TaskInList { Id = idTask, Description = descruptionTask, Alias = aliasTask, Status = statusTask });
            Console.WriteLine("press 1 to add dependecy or 0 to continue:");
            check = GetInteger();
        }

        Console.Write("Required Effort Time (days): ");
        TimeSpan requiredEffortTime = TimeSpan.FromDays(double.Parse(Console.ReadLine()));

        Console.Write("Start date (in the format dd/mm/yyyy): ");//receive start date (additional)
        DateTime? tempDate = GetDateTime();//recive and check date
        DateTime? startDate = (tempDate == null) ? null : tempDate;

        Console.Write("Scheduled date (in the format dd/mm/yyyy): "); //receive Scheduled date (additional)
        tempDate = GetDateTime();//recive and check date
        DateTime? scheduledDate = (tempDate == null) ? null : tempDate;

        Console.Write("DeadLine date (in the format dd/mm/yyyy): ");  //receive deadline date (additional)
        tempDate = GetDateTime();//recive and check date
        DateTime? deadLine = (tempDate == null) ? null : tempDate;

        Console.Write("Complete date (in the format dd/mm/yyyy): ");  //receive complete date (additional)
        tempDate = GetDateTime();
        DateTime? completeDate = (tempDate == null) ? null : tempDate;

        Console.Write("Deliverables: ");
        string? deliverables = Console.ReadLine();

        Console.Write("Remarks: ");
        string? remark = Console.ReadLine();

        Console.WriteLine("Enter Engineer's details: ");
        Console.Write("ID: "); int idEngneer = GetInteger();
        Console.Write("Name: "); string nameEngineer = Console.ReadLine();
        BO.EngineerInTask? engineer = new BO.EngineerInTask { Id = idEngneer, Name = nameEngineer };

        Console.Write("Enter Task's complexity, Rating between 1-5: ");
        BO.EngineerExperience complexity = (BO.EngineerExperience)(checkNum());

        BO.Task item = new BO.Task
        {
            Id = id,
            Alias = alias,
            Description = description,
            CreatedAtDate = DateTime.Now,
            Status = status,
            Dependencies = dependency,
            Milestone = null,
            RequiredEffortTime = requiredEffortTime,
            StartDate = startDate,
            ScheduledDate = scheduledDate,
            ForecastDate = scheduledDate + requiredEffortTime,
            DeadlineDate = deadLine,
            Deliverables = deliverables,
            Remarks = remark,
            Engineer = engineer,
            Complexity = complexity
        };
        return item;
    }
    private int checkNum()
    {
        int level;
        do
        {
            level = GetInteger();
            Console.WriteLine("ERROR: choose level between 1-5");

        } while (level < 1 || level > 5);
        return level;
    }
    private int GetInteger()
    {
        int input = 0;
        while (!int.TryParse(Console.ReadLine(), out input))
            Console.WriteLine("Expecting only integer now!");
        return input;
    }
    private DateTime GetDateTime()
    {
        DateTime input;
        while (!DateTime.TryParse(Console.ReadLine(), out input))
            Console.WriteLine("Expecting only DateTime now!");
        return input;
    }
   
}

