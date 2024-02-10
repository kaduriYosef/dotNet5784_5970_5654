namespace BO;

/// <summary>
/// Represents a task as assigned to an engineer, containing minimal information required for identification and reference.
/// </summary>
public class TaskInEngineer
{
    /// <summary>
    /// Gets the unique identifier of the task.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets or sets the alias of the task, a short, identifiable name.
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// Generates a string representation of the TaskInEngineer object, including its ID and alias.
    /// </summary>
    /// <returns>A string detailing the ID and alias of the task.</returns>
    public override string ToString() => this.ToStringProperty();
}
