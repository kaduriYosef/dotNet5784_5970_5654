namespace BO;

/// <summary>
/// Represents a milestone related to a specific task, identifying key points or achievements within the task's lifecycle.
/// </summary>
public class MilestoneInTask
{
    /// <summary>
    /// Gets the unique identifier for the milestone.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets or sets the alias or name of the milestone, providing a brief description or title.
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// Overrides the ToString method to provide a string representation of the MilestoneInTask object,
    /// showcasing its properties in a formatted manner.
    /// </summary>
    /// <returns>A string representation of the MilestoneInTask object.</returns>
    public override string ToString() => this.ToStringProperty();
}
