namespace BO;

/// <summary>
/// Represents an engineer, including their basic information, experience level, and associated tasks.
/// This class is designed to encapsulate all relevant details about an engineer within the system.
/// </summary>
public class Engineer
{
    /// <summary>
    /// Gets the unique identifier for the engineer. This value is immutable once the engineer is created.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets or sets the name of the engineer. This property can be modified after the engineer is created.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the email address of the engineer. This is used for communication and identification within the system.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the experience level of the engineer, categorized by the EngineerExperience enum. This impacts task assignment and management.
    /// </summary>
    public EngineerExperience Level { get; set; }

    /// <summary>
    /// Gets or sets the hourly cost of the engineer. This is used for budgeting and project cost calculations.
    /// </summary>
    public double Cost { get; set; }

    /// <summary>
    /// Optional. Gets or sets the primary task currently assigned to the engineer. This property can be null if no task is assigned.
    /// </summary>
    public TaskInEngineer? Task { get; set; }

    /// <summary>
    /// Optional. A list of additional tasks assigned to the engineer. This allows for multiple task assignments beyond the primary task.
    /// </summary>
    public List<TaskInEngineer>? AdditionalTasks { get; set; }

    /// <summary>
    /// Overrides the ToString method to provide a string representation of the Engineer object,
    /// including its ID, name, email, level, and associated tasks.
    /// </summary>
    /// <returns>A string detailing the engineer's information and tasks.</returns>
    public override string ToString() => this.ToStringProperty();
}
