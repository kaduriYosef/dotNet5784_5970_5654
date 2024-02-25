using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace BO;

/// <summary>
/// Provides utility methods for transforming objects and calculating status within the business object layer.
/// </summary>
static public class Tools
{

    #region to string property

    /// <summary>
    /// Generates a string representation of an object's properties, handling primitive types, strings, enumerables, and complex object types recursively.
    /// </summary>
    /// <param name="obj">The object to generate a string representation for.</param>
    /// <returns>A string detailing the properties and their values of the object.</returns>
    public static string ToStringProperty(this object obj) => ToStringProperty(obj, 0);

    /// <summary>
    /// Internal method to generate a string representation of an object's properties, including handling of nested objects up to a specified depth.
    /// </summary>
    /// <param name="obj">The object to generate a string representation for.</param>
    /// <param name="depth">The current depth of recursion, used for indentation purposes.</param>
    /// <returns>A string detailing the properties and their values of the object, considering the current depth.</returns>
    private static string ToStringProperty(object obj, int depth)
    {
        if (obj == null) return "null";
        if (obj.GetType().IsPrimitive || obj is string) return $"{new String('\t', depth)}{obj}";

        Type type = obj.GetType();
        StringBuilder sb = new StringBuilder();
        if (depth > 0)
        {
            sb.AppendLine($"{new String('\t', depth)}Type: {type.Name}");
        }
        else
        {
            sb.AppendLine($"Type: {type.Name}");
        }

        foreach (PropertyInfo property in type.GetProperties())
        {
            if (property.GetGetMethod() != null && !property.GetIndexParameters().Any())
            {
                object? value = property.GetValue(obj, null);
                var propertyIndentation = new String('\t', depth);
                if (value is System.Collections.IEnumerable && !(value is string) && !(value.GetType().IsEnum))
                {
                    sb.AppendLine($"{propertyIndentation}{property.Name}:");
                    foreach (var item in (System.Collections.IEnumerable)value)
                    {
                        if (item.GetType().IsPrimitive || item is string)
                        {
                            sb.AppendLine($"{new String('\t', depth + 1)}{item}");
                        }
                        else
                        {
                            sb.Append(ToStringProperty(item, depth + 1));
                        }
                        sb.Append('\n');
                    }
                }
                else
                {
                    if (value == null)
                    {
                        sb.AppendLine($"{propertyIndentation}{property.Name}: null");
                    }
                    else if (value is string || value.GetType().IsPrimitive || value.GetType().IsEnum || value is DateTime ||value is TimeSpan)
                    {
                        sb.AppendLine($"{propertyIndentation}{property.Name}: {value}");
                    }
                    else
                    {
                        sb.AppendLine($"{propertyIndentation}{property.Name}:");
                        sb.Append(ToStringProperty(value, depth + 1));
                    }
                }
            }
        }
        return sb.ToString();
    }
    #endregion

    #region simplify Engineer

    /// <summary>
    /// Converts a BO.Engineer object to a BO.EngineerInTask object.
    /// </summary>
    /// <param name="itemBoEngineer">The BO.Engineer object to convert.</param>
    /// <returns>A BO.EngineerInTask object or null if the input is null.</returns>
    static internal BO.EngineerInTask? fromEngineerToEngineerInTask(BO.Engineer? itemBoEngineer)
    {
        if (itemBoEngineer == null) return null;
        return new BO.EngineerInTask
        {
            Id = itemBoEngineer.Id,
            Name = itemBoEngineer.Name,
        };
    }

    /// <summary>
    /// Converts a DO.Engineer object to a BO.EngineerInTask object.
    /// </summary>
    /// <param name="itemDoEngineer">The DO.Engineer object to convert.</param>
    /// <returns>A BO.EngineerInTask object or null if the input is null.</returns>
    static public BO.EngineerInTask? fromEngineerToEngineerInTask(DO.Engineer? itemDoEngineer)
    {
        if (itemDoEngineer == null) return null;
        return new BO.EngineerInTask
        {
            Id = itemDoEngineer.Id,
            Name = itemDoEngineer.Name,
        };
    }
    #endregion

    /// <summary>
    /// Calculates the status of a task based on its scheduled, start, and completion dates.
    /// </summary>
    /// <param name="doTask">The task for which to calculate the status.</param>
    /// <returns>The calculated status of the task.</returns>
    public static BO.Status CalcStatus(DO.Task doTask)
    {
        if (doTask.ScheduledDate is null) return BO.Status.Unscheduled;
        if (doTask.StartDate is null) return BO.Status.Scheduled;
        if (doTask.CompleteDate is null)
            if (doTask.DeadlineDate < DateTime.Now)
                return BO.Status.InJeopardy;
            else
                return BO.Status.OnTrack;
        return BO.Status.Done;
    }

    #region simplify tasks
    /// <summary>
    /// Converts a DO.Task object to a BO.TaskInList object.
    /// </summary>
    /// <param name="doTask">The DO.Task object to convert.</param>
    /// <returns>A BO.TaskInList object or null if the input is null.</returns>
    static public BO.TaskInList? fromTaskToTaskInList(DO.Task? doTask)
    {
        if (doTask is null) return null;
        return new BO.TaskInList
        {
            Id = doTask.Id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            Status = CalcStatus(doTask)
        };
    }

    /// <summary>
    /// Converts a BO.Task object to a BO.TaskInList object.
    /// </summary>
    /// <param name="boTask">The BO.Task object to convert.</param>
    /// <returns>A BO.TaskInList object or null if the input is null.</returns>
    static public BO.TaskInList? fromTaskToTaskInList(BO.Task? boTask)
    {
        if (boTask is null) return null;
        return new BO.TaskInList
        {
            Id = boTask.Id,
            Alias = boTask.Alias,
            Description = boTask.Description,
            Status = boTask.Status
        };
    }

    /// <summary>
    /// Converts a DO.Task object to a BO.TaskInEngineer object.
    /// </summary>
    /// <param name="doTask">The DO.Task object to convert.</param>
    /// <returns>A BO.TaskInEngineer object or null if the input is null.</returns>
    static public BO.TaskInEngineer? fromTaskToTaskInEngineer(DO.Task? doTask)
    {
        if (doTask is null) return null;
        return new BO.TaskInEngineer
        {
            Id = doTask.Id,
            Alias = doTask.Alias
        };
    }

    /// <summary>
    /// Converts a BO.Task object to a BO.TaskInEngineer object.
    /// </summary>
    /// <param name="boTask">The BO.Task object to convert.</param>
    /// <returns>A BO.TaskInEngineer object or null if the input is null.</returns>
    static public  BO.TaskInEngineer? fromTaskToTaskInEngineer(BO.Task? boTask)
    {
        if (boTask is null) return null;
        return new BO.TaskInEngineer
        {
            Id = boTask.Id,
            Alias = boTask.Alias
        };
    }
    #endregion
}
