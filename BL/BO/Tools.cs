
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace BO;

static public class Tools
{
    #region to string property
    public static string ToStringProperty(this object obj) => ToStringProperty(obj, 0);

    private static string ToStringProperty(object obj, int depth)
    {
        if (obj == null) return "null";
        if (obj.GetType().IsPrimitive || obj is string) return $"{new String('\t', depth )}{obj}";

        Type type = obj.GetType();
        StringBuilder sb = new StringBuilder();
        if (depth > 0)
        {
            sb.AppendLine($"{new String('\t', depth )}Type: {type.Name}");
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
                var propertyIndentation = new String('\t', depth );
                if (value is System.Collections.IEnumerable && !(value is string)&& !(value.GetType().IsEnum ))
                {
                    sb.AppendLine($"{propertyIndentation}{property.Name}:");
                    foreach (var item in (System.Collections.IEnumerable)value)
                    {
                        if (item.GetType().IsPrimitive || item is string)
                        {
                            sb.AppendLine($"{new String('\t', depth  + 1)}{item}");
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
                    if(value == null)
                    {
                        sb.AppendLine($"{propertyIndentation}{property.Name}: null");
                    }
                    else if (value is string || value.GetType().IsPrimitive|| value.GetType().IsEnum||value is DateTime)
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

    static internal BO.EngineerInTask? fromEngineerToEngineerInTask(BO.Engineer? itemBoEngineer)
    {
        if (itemBoEngineer == null) return null;
        return new BO.EngineerInTask
        {
            Id = itemBoEngineer.Id,
            Name = itemBoEngineer.Name,
        };
    }
    static internal BO.EngineerInTask? fromEngineerToEngineerInTask(DO.Engineer? itemDoEngineer)
    {
        if (itemDoEngineer == null) return null;
        return new BO.EngineerInTask
        {
            Id = itemDoEngineer.Id,
            Name = itemDoEngineer.Name,
        };
    }



    #endregion

    internal static BO.Status CalcStatus(DO.Task doTask)
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
    static internal BO.TaskInList? fromTaskToTaskInList(DO.Task? doTask)
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
    static internal BO.TaskInList? fromTaskToTaskInList(BO.Task? boTask)
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

    static internal BO.TaskInEngineer? fromTaskToTaskInEngineer(DO.Task? doTask)
    {
        if (doTask is null) return null;
        return new BO.TaskInEngineer
        {
            Id = doTask.Id,
            Alias = doTask.Alias
        };


    }
    static internal BO.TaskInEngineer? fromTaskToTaskInEngineer(BO.Task? boTask)
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
