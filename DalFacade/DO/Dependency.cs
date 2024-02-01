namespace DO;

/// <summary>
/// Captures task dependencies in a project.
/// </summary>
/// <param name="Id">Identifier for the dependency record.</param>
/// <param name="DependentTask">ID of the task that relies on another task.</param>
/// <param name="DependsOnTask">ID of the task being relied upon.</param>
public record Dependency
(
    int Id,
    int DependentTask=0,
    int DependsOnTask=0
)
{
    // Empty constructor
    public Dependency() : this(0)
    { }
    public Dependency(int dependent,int dependsOn):this(0,dependent,dependsOn)
    { }
}
