using System.Xml;
using System;
using System.ComponentModel;
using System.Collections.Generic; // Ensure this is included for List<T>

namespace BO;

/// <summary>
/// Represents a detailed task within a project management context, encompassing all necessary information for tracking and execution.
/// </summary>
public class Task
{
    /// <summary>Unique identifier for the task.</summary>
    public int Id { get; init; }

    /// <summary>Short, memorable name of the task.</summary>
    public string Alias { get; set; }

    /// <summary>Detailed description of what the task entails.</summary>
    public string Description { get; set; }

    /// <summary>Date and time when the task was created.</summary>
    public DateTime CreatedAtDate { get; init; }

    /// <summary>Current status of the task, indicating its progress.</summary>
    public Status? Status { get; set; }

    /// <summary>List of other tasks that this task depends on.</summary>
    public List<TaskInList> Dependencies { get; set; }

    /// <summary>Associated milestone for the task, if any.</summary>
    public MilestoneInTask? Milestone { get; set; }

    /// <summary>Estimated effort required to complete the task.</summary>
    public TimeSpan? RequiredEffortTime { get; set; }

    /// <summary>Date and time when the task is started.</summary>
    public DateTime? StartDate { get; set; }

    /// <summary>Date and time when the task is scheduled to start.</summary>
    public DateTime? ScheduledDate { get; set; }

    /// <summary>Forecasted completion date based on current progress.</summary>
    public DateTime? ForecastDate { get; set; }

    /// <summary>Deadline by which the task must be completed.</summary>
    public DateTime? DeadlineDate { get; set; }

    /// <summary>Date and time when the task was completed.</summary>
    public DateTime? CompleteDate { get; set; }

    /// <summary>Deliverables or outputs expected upon task completion.</summary>
    public string? Deliverables { get; set; }

    /// <summary>Additional remarks or notes related to the task.</summary>
    public string? Remarks { get; set; }

    /// <summary>The engineer assigned to the task.</summary>
    public EngineerInTask? Engineer { get; set; }

    /// <summary>The complexity level of the task, as assessed by the assigned engineer.</summary>
    public EngineerExperience? Complexity { get; set; }

    /// <summary>
    /// Provides a string representation of the task, detailing its properties for easy reading.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => this.ToStringProperty();
}
