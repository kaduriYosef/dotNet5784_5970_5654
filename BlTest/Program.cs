namespace BlTest;

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void Main(string[] args)
    {

        BO.Engineer engineer = new BO.Engineer()
        {
            Email="email",
            Id=1,
            Cost=1.45,
            Level=BO.EngineerExperience.Intermediate,
            Task=new BO.TaskInEngineer() { Id=1000,Alias="die" },
        };
        Console.WriteLine(engineer);
        return;
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();

       

    }
}
