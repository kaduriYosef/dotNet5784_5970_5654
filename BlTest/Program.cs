using DalTest;

namespace BlTest;

using System.Reflection.Emit;
using BO; // Assuming the namespace for the classes

static class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();


    public static void Main(string[] args)
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
                    case 3: //create schedule
                        DateTime? startProject = null;
                        Console.WriteLine("Enter A Project Start Date");
                        startProject = GetDateTime();
                        //We will activate the function that generates the start dates of all the tasks
                        s_bl.Task.ScheduleAllDates(startProject.GetValueOrDefault());
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
    static int menuChoise()
    {
        int choice;
        Console.WriteLine("Press - 0 exit");
        Console.WriteLine("Press - 1 for Engineer's entity");
        Console.WriteLine("Press - 2 for Task's entity");
        choice = GetInteger();
        return choice;
    }
    static int subMenuChoise(string type)
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
    static int choiceInEntity(int userChoice, int EngTask)
    {
        do
        {
            switch (userChoice)
            {
                case 0:
                    break;
                case 1:
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
                        foreach (var item1 in (s_bl.Engineer.ReadAll().ToList()))
                        {
                            Console.WriteLine(item1);
                            Console.WriteLine();
                        }
                    else if (EngTask == 2)
                        foreach (var item2 in s_bl.Task.ReadAllSimplified().ToList())
                        {
                            Console.WriteLine(item2);
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

                    s_bl.Task.ScheduledDateManagement(id, scheduledDate);
                    break;

                default:  //if the user choose wrong number 
                    Console.WriteLine("ERORR: choose numbers betwin 1-6");
                    userChoice = GetInteger();
                    break;
            }
        } while (userChoice < 0 || userChoice > 5);
        return 1;
    }
    static BO.Engineer GetEngineer()
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
        Console.Write("Task's ID for engineer: ");
        BO.TaskInEngineer? taskInEngineer = new BO.TaskInEngineer();
        try
        {
            BO.Task? task = s_bl.Task.Read(GetInteger());
            taskInEngineer = new BO.TaskInEngineer
            {
                Id = task.Id,
                Alias = task.Alias
            };

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

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
    static BO.Task GetTask()
    {
        Console.WriteLine("Enter Task details:");
        Console.Write("ID: ");
        int id = GetInteger();

        Console.Write("Alias: ");
        string alias = Console.ReadLine() ?? "";

        Console.Write("Description: ");
        string description = Console.ReadLine() ?? "";

        Console.WriteLine("press 1 to add dependecy or 0 to skip:");
        int check = GetInteger();
        List<BO.TaskInList>? dependency = new List<BO.TaskInList>();
        BO.Task task = new BO.Task();
        while (check == 1) //get all the parameter for all the dependcies of this task 
        {
            Console.Write("Enter ID of dependent task: ");
            int idTask = GetInteger();
            try
            {
                task = s_bl.Task.Read(GetInteger());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            dependency.Add(new BO.TaskInList
            {
                Id = task.Id,
                Description = task.Description,
                Alias = task.Alias,
                Status = task.Status,
            });
            Console.WriteLine("press 1 to add dependecy or 0 to continue:");
            check = GetInteger();
        }

        Console.Write("Required Effort Time (days): ");
        TimeSpan requiredEffortTime = TimeSpan.FromDays(GetInteger());

        Console.Write("Deliverables: ");
        string? deliverables = Console.ReadLine();

        Console.Write("Remarks: ");
        string? remark = Console.ReadLine();

        Console.WriteLine("Enter Engineer's ID for task: ");

        BO.EngineerInTask engInTask = new BO.EngineerInTask();
        try
        {
            BO.Engineer? eng = s_bl.Engineer.Read(GetInteger());
            engInTask = new BO.EngineerInTask
            {
                Id = eng.Id,
                Name = eng.Name
            };

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }


        Console.Write("Enter Task's complexity, Rating between 1-5: ");
        BO.EngineerExperience complexity = (BO.EngineerExperience)(checkNum());

        BO.Task item = new BO.Task
        {
            Id = id,
            Alias = alias,
            Description = description,
            CreatedAtDate = DateTime.Now,
            Status = null,
            Dependencies = dependency,
            Milestone = null,
            RequiredEffortTime = requiredEffortTime,
            StartDate = null,
            ScheduledDate = null,
            ForecastDate = null,
            DeadlineDate = null,
            Deliverables = deliverables,
            Remarks = remark,
            Engineer = engInTask,
            Complexity = complexity
        };
        return item;
    }
    static private int checkNum()
    {
        int level;
        do
        {
            level = GetInteger();
            Console.WriteLine("ERROR: choose level between 0-4");

        } while (level <= 0 || level > 4);
        return level;
    }
    static private int GetInteger()
    {
        int input = 0;
        while (!int.TryParse(Console.ReadLine(), out input))
            Console.WriteLine("Expecting only integer now!");
        return input;
    }
    static private DateTime GetDateTime()
    {
        DateTime input;
        while (!DateTime.TryParse(Console.ReadLine(), out input))
            Console.WriteLine("Expecting only DateTime now!");
        return input;
    }

}

