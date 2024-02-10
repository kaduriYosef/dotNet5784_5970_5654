using System.ComponentModel;

namespace BO;

/// <summary>
/// Represents a simplified task for listing purposes with basic details including ID, description, alias, and status.
/// </summary>
public class TaskInList
{
    /// <summary>Unique identifier of the task.</summary>
    public int Id { get; init; }

    /// <summary>Description of the task.</summary>
    public string Description { get; set; }

    /// <summary>Short, memorable name of the task.</summary>
    public string Alias { get; set; }

    /// <summary>Current progress or state of the task.</summary>
    public Status? Status { get; set; }

    /// <summary>Returns a string representation of the TaskInList object, detailing its properties.</summary>
    /// <returns>Formatted string of task details.</returns>
    public override string ToString() => this.ToStringProperty();
}
