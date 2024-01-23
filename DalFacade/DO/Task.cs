
namespace DO;
/// <summary>
/// Represents a task, detailing key aspects and tracking in a project.
/// </summary>
/// <param name="Id">Task's unique identifier.</param>
/// <param name="Alias">Short, optional name for quick reference.</param>
/// <param name="Description">Brief description of the task.</param>
/// <param name="CreatedAtDate">Date and time of task creation.</param>
/// <param name="RequiredEffortTime">Estimated time to complete the task.</param>
/// <param name="IsMilestone">Indicates if the task is a major project milestone.</param>
/// <param name="Complexity">Complexity level, based on required engineering expertise.</param>
/// <param name="StartDate">Scheduled start date for the task.</param>
/// <param name="ScheduledDate">Planned date for task execution.</param>
/// <param name="DeadlineDate">Deadline for task completion.</param>
/// <param name="CompleteDate">Actual completion date of the task.</param>
/// <param name="Deliverables">Expected outputs upon completion.</param>
/// <param name="Remarks">Additional notes or comments about the task.</param>
/// <param name="EngineerId">ID of the responsible engineer.</param>

public record Task
(
    int Id,
    string Alias,
    string Description  ,
    DateTime CreatedAtDate,
    TimeSpan? RequiredEffortTime = null,
    bool IsMilestone=false,
    EngineerExperience? Complexity = null,
    DateTime? StartDate = null,
    DateTime? ScheduledDate = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate=null,
    string? Deliverables = null,
    string? Remarks = null,
    int? EngineerId=null
    )
{
    
    //Empty Ctor
    public Task() : this(0,"","", DateTime.Now)
    { }
}
