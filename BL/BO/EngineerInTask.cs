namespace BO;

/// <summary>
/// Represents an engineer assigned to a specific task, including basic identification information.
/// </summary>
public class EngineerInTask
{
    /// <summary>
    /// Gets the unique identifier for the engineer.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Gets the name of the engineer. This property is initialized upon object creation and cannot be changed thereafter.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Generates a string representation of the EngineerInTask object, including its ID and name.
    /// </summary>
    /// <returns>A string detailing the engineer's ID and name.</returns>
    public override string ToString() => this.ToStringProperty();
}
