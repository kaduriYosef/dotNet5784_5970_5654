namespace BlTest;
using BO; // Assuming the namespace for the classes

class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void Main()
    {
        Console.Write("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
        if (ans == "Y")
            DalTest.Initialization.Do();

    }
}
