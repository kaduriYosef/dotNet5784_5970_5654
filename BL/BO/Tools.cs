using System.ComponentModel;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Net.Mail;
using System.Xml;

namespace BO;

/// <summary>
/// Provides utility methods for transforming objects and calculating status within the business object layer.
/// </summary>
static public class Tools
{


    internal static void IsValidEmail(string email)
    {
        try
        {
            MailAddress mailAddress = new MailAddress(email);
            
        }
        catch
        {
            throw new BlInvalidDataException("email is invalid");
        }
    }

    #region to string property
    /// <summary>
    /// returns whether the obj is a primitive string or we should look into it
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private static bool isPrimitiveString(object obj)
    {
        if (obj.GetType().IsPrimitive || obj is string || obj.GetType().IsEnum || obj is DateTime || obj is TimeSpan)
            return true;
        else return false;
    }

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
        //if (obj.GetType().IsPrimitive || obj is string || obj.GetType().IsEnum || obj is DateTime || obj is TimeSpan) 
        //    return $"{new String('\t', depth)}{obj}";
        if (isPrimitiveString(obj))
            return $"{new String('\t', depth)}{obj}";


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
                        if (isPrimitiveString(item))
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
                    else if (isPrimitiveString(value))
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

    #region help function with xml
    public static DateTime? StartDateOrNull()
    {
        // Define the path to your XML file
        string path = @"C:\Users\User\source\repos\מיני פרוייקט\dotNet5784_5970_5654\xml\data-config.xml";

        // Load the XML document
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        // Find the <StartDate> element
        XmlNode? startDateNode = xmlDoc.SelectSingleNode("//StartDate");
        if (startDateNode != null)
        {
            // Check if the <StartDate> value is null or empty
            if (string.IsNullOrEmpty(startDateNode.InnerText))
            {
                return null;
            }
            else
            {
                // <StartDate> has a value
                return DateTime.Parse(startDateNode.InnerText);
            }
        }
        else
        {
            Console.WriteLine("StartDate element not found.");
            return null;
        }

    }

    public static void update_StartDate_unsafe(DateTime date)
    {
        // Define the path to your XML file
        string path = @"C:\Users\User\source\repos\מיני פרוייקט\dotNet5784_5970_5654\xml\data-config.xml";


        // Load the XML document
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        // Find the <StartDate> element
        XmlNode? startDateNode = xmlDoc.SelectSingleNode("//StartDate");
        if (startDateNode != null)
        {
            // Update the <StartDate> value to the new date
            startDateNode.InnerText = date.ToString();

            // Save the changes back to the file
            xmlDoc.Save(path);

        }
        else
        {
            Console.WriteLine("StartDate element not found.");
            throw new BlDoesNotExistException("StartDate was not found!");
        }
    }
    #endregion
}
