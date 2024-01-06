namespace DO;
/// <summary>
/// Encapsulates configuration details for a project or activity.
/// </summary>
/// <param name="StartDate">The commencement date of the project/activity.</param>
/// <param name="EndDate">The targeted completion date of the project/activity.</param>
/// <param name="Status">The current progress status of the project/activity.</param>
public record Config
(
    DateTime? StartDate=null,
    DateTime? EndDate = null,
    Status? Status = null
);
