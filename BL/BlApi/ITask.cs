namespace BlApi;

/// <summary>
/// Defines the interface for task management, extending basic CRUD operations and including specialized task functionalities.
/// </summary>
public interface ITask : ICrud<BO.Task>
{
    /// <summary>
    /// Reads and returns a simplified list of tasks, optionally filtered by a provided condition.
    /// </summary>
    /// <param name="filter">An optional filter to apply to tasks. If null, all tasks are returned.</param>
    /// <returns>A list of tasks in a simplified format.</returns>
    public IEnumerable<BO.TaskInList> ReadAllSimplified(Func<BO.Task, bool>? filter = null);

    /// <summary>
    /// Manages the scheduled date for a specific task.
    /// </summary>
    /// <param name="id">The ID of the task to update.</param>
    /// <param name="date">The new scheduled date for the task.</param>
    public void ScheduledDateManagement(int id, DateTime date);

    /// <summary>
    /// Schedules the dates for all tasks starting from the given start date of the project.
    /// </summary>
    /// <param name="startOfProject">The start date from which to schedule all tasks.</param>
    public void ScheduleAllDates(DateTime startOfProject);
}
