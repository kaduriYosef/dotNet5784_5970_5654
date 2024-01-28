
using System.Xml;
using System;

namespace BO;

public class Task
{

    public int Id { get; init; }
    public string Description;
    public string Alias;
    public DateTime CreatedAtDate;
    public Status? Status;
    public List<TaskInList>? Dependencies;
    public MilestoneInTask? Milestone;
    public TimeSpan? RequiredEffortTime;
    public DateTime? StartDate;
    public DateTime? ScheduledDate;
    public DateTime? ForecastDate;
    public DateTime? DeadlineDate;
    public DateTime? CompleteDate;
    public string? Deliverables;
    public string? Remarks;
    public EngineerInTask Engineer;
    public EngineerExperience Copmlexity;


}

