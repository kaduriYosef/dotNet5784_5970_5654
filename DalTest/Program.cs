 namespace DalTest;

using Dal;
using DalApi;
internal class Program
{
    private static ITask? s_dalStudent = new TaskImplementation(); //stage 1
    private static IEngineer? s_dalCourse = new EngineerImplementation(); //stage 1
    private static IDependency? s_dalLinks = new DependencyImplementation(); //stage 1
    static void Main(string[] args)
    {
    
    }
}
