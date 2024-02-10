namespace BO;

/// <summary>
/// Represents a milestone for listing purposes, encapsulating essential details such as its identifier, description, status, and completion percentage.
/// </summary>
public class MilestoneInList
{
    /// <summary>
    /// Gets the unique identifier of the milestone.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets or sets the detailed description of the milestone, explaining its objectives or key results.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the alias or short name of the milestone for easy reference.
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// Gets or sets the current status of the milestone, indicating its progress towards completion.
    /// </summary>
    public Status? Status { get; set; }

    /// <summary>
    /// Gets or sets the completion percentage of the milestone, reflecting how much of the milestone has been achieved.
    /// </summary>
    public double? CompletionPercentage { get; set; }

    /// <summary>
    /// Generates a string representation of the MilestoneInList object, including its basic properties.
    /// </summary>
    /// <returns>A string detailing the milestone's ID, description, alias, status, and completion percentage.</returns>
    public override string ToString() => this.ToStringProperty();
}
