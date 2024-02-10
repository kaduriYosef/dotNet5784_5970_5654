namespace BO;

/// <summary>
/// Represents a milestone within a project, detailing its key attributes including creation, status, deadlines, and completion.
/// </summary>
public class Milestone
{
    /// <summary>
    /// Gets the unique identifier of the milestone.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets or sets the detailed description of what the milestone entails.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets the alias or short name of the milestone for easy reference.
    /// </summary>
    public string Alias { get; set; }

    /// <summary>
    /// Gets the date and time when the milestone was created.
    /// </summary>
    public DateTime CreatedAtDate { get; init; }

    /// <summary>
    /// Gets or sets the current status of the milestone, indicating its progress or completion state.
    /// </summary>
    public Status? Status { get; set; }

    /// <summary>
    /// Gets or sets the forecasted date for when the milestone is expected to be completed.
    /// </summary>
    public DateTime? ForecastDate { get; set; }

    /// <summary>
    /// Gets or sets the deadline date by which the milestone should be completed.
    /// </summary>
    public DateTime? DeadlineDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the milestone was actually completed.
    /// </summary>
    public DateTime? CompleteDate { get; set; }

    /// <summary>
    /// Gets or sets the completion percentage of the milestone, reflecting how much of it has been achieved.
    /// </summary>
    public double? CompletionPercentage { get; set; }

    /// <summary>
    /// Gets or sets any additional remarks or notes related to the milestone.
    /// </summary>
    public string? Remarks { get; set; }

    /// <summary>
    /// Gets or sets a list of tasks (as TaskInList objects) that are dependencies for the milestone.
    /// </summary>
    public List<TaskInList>? Dependencies { get; set; }

    /// <summary>
    /// Overrides the ToString method to provide a string representation of the Milestone object,
    /// detailing its key properties in a readable format.
    /// </summary>
    /// <returns>A string representation of the Milestone object.</returns>
    public override string ToString() => this.ToStringProperty();
}
