using DalTest;

namespace BlTest;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    
    
    static void Main(string[] args)
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
                            int choice1 = subMenuChoise("Engineer");
                            choiceInEntity(choice1, 1);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case 2:     //Task entity
                        try
                        {
                            int choice2 = subMenuChoise("Task");
                            choiceInEntity(choice2, 2);
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    default:    
                        Console.WriteLine("ERROR: choose number between 1-3");
                        chose = int.Parse(Console.ReadLine()!);
                        break;
                }
                Console.WriteLine();
            } while (chose != 0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        int menuChoise()
        {
            int choice;
            Console.WriteLine("Press - 0 exit");
            Console.WriteLine("Press - 1 for Engineer's entity");
            Console.WriteLine("Press - 2 for Task's entity");
            choice = int.Parse(Console.ReadLine()!);
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

            return int.Parse(Console.ReadLine()!);
        }

        static int choiceInEntity(int userChoice, int EngTask)
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
                            printEng(s_bl.Engineer.Read(int.Parse(Console.ReadLine()!)));
                        }
                        else if (EngTask == 2)
                        {
                            Console.Write("Enter Task's ID: ");
                            printTask(s_bl.Task.Read(int.Parse(Console.ReadLine())));
                        }
                        break;
                    case 3: //read all
                        if (EngTask == 1)
                            foreach (var item1 in s_bl.Engineer.ReadAll())
                            {
                                printEng(item1);
                                Console.WriteLine();
                            }
                        else if (EngTask == 2)
                            foreach (var item2 in s_bl.Task.ReadAll())
                            {
                                Console.WriteLine($"Task ID: {item2.Id}");
                                Console.WriteLine($"Description: {item2.Description}");
                                Console.WriteLine($"Alias: {item2.Alias}");
                                Console.WriteLine($"Status: {item2.Status}");
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
                                s_bl.Engineer.Delete(int.Parse(Console.ReadLine()));
                            }
                            else if (EngTask == 2)
                            {
                                Console.Write("Enter Task's ID: ");
                                s_bl.Task.Delete(int.Parse(Console.ReadLine()));
                            }
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case 6://update date

                        Console.Write("Enter Task's ID: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.Write("Enter Task's Scheduled date: ");
                        string? tempDate = GetDate();//recive and check date
                        if (tempDate == null)//the date didnt defined
                        {
                            Console.WriteLine("Date didn't defined");
                            break;
                        }
                        DateTime scheduledDate = DateTime.Parse(tempDate);
                        s_bl.Task.UpdateDate(id, scheduledDate);
                        break;

                    default:  //if the user choose wrong number 
                        Console.WriteLine("ERORR: choose numbers betwin 1-6");
                        userChoice = int.Parse(Console.ReadLine());
                        break;
                }
            } while (userChoice < 0 || userChoice > 5);
            return 1;
            //BO.Engineer engineer = new BO.Engineer()
            //{
            //    Email="email",
            //    Id=1,
            //    Cost=1.45,
            //    Level=BO.EngineerExperience.Intermediate,
            //    Task=new BO.TaskInEngineer() { Id=1000,Alias="die" },
            //};
            //Console.WriteLine(engineer);
            //return;
            //Console.Write("Would you like to create Initial data? (Y/N)");
            //string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            //if (ans == "Y")
            //    DalTest.Initialization.Do();



        }
}
