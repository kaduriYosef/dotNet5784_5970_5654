
namespace Dal;

internal static class DataSource
{
    internal static class Config
    {
        //for task class
        internal const int startTaskId = 0;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }

        //for Dependency class
        internal const int startDependencyId = 0;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
    }
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();


}
