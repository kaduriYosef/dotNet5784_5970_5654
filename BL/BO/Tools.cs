using System.ComponentModel;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Net.Mail;
using System.Xml;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            return $"{new System.String('\t', depth)}{obj}";


        Type type = obj.GetType();
        StringBuilder sb = new StringBuilder();
        if (depth > 0)
        {
            sb.AppendLine($"{new System.String('\t', depth)}Type: {type.Name}");
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
                var propertyIndentation = new System.String('\t', depth);
                if (value is System.Collections.IEnumerable && !(value is string) && !(value.GetType().IsEnum))
                {
                    sb.AppendLine($"{propertyIndentation}{property.Name}:");
                    foreach (var item in (System.Collections.IEnumerable)value)
                    {
                        if (isPrimitiveString(item))
                        {
                            sb.AppendLine($"{new System.String('\t', depth + 1)}{item}");
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

    private static string path_to_data_config = @"..\xml\data-config.xml";

    #region start date
    public static DateTime? StartDateOrNull()
    { 

        // Load the XML document
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path_to_data_config);

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
                //return DateTime.TryParse(startDateNode.InnerText);
                if (DateTime.TryParse(startDateNode.InnerText, out DateTime result))
                {
                    return result;
                }
                else
                {
                    // Handle the situation where the date could not be parsed
                    return null;
                }

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
        // Load the XML document
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path_to_data_config);

        // Find the <StartDate> element
        XmlNode? startDateNode = xmlDoc.SelectSingleNode("//StartDate");
        if (startDateNode != null)
        {
            // Update the <StartDate> value to the new date
            startDateNode.InnerText = date.ToString();

            // Save the changes back to the file
            xmlDoc.Save(path_to_data_config);

        }
        else
        {
            throw new BlDoesNotExistException("StartDate was not found!");
        }
    }

    #endregion

    //#region clock
    //public static void setClock(DateTime clock)
    //{
    //    // Load the XML document
    //    XmlDocument xmlDoc = new XmlDocument();
    //    xmlDoc.Load(path_to_data_config);

    //    // Find the <Clock> element
    //    XmlNode? clockNode = xmlDoc.SelectSingleNode("//Clock");
    //    if (clockNode != null)
    //    {
    //        // Update the <Clock> value to the new date
    //        clockNode.InnerText = clock.ToString();

    //        // Save the changes back to the file
    //        xmlDoc.Save(path_to_data_config);

    //    }
    //    else
    //    {
    //        throw new BlDoesNotExistException("Clock was not found!");
    //    }
    //}
    //static public DateTime getClock()
    //{
    //    // Load the XML document
    //    XmlDocument xmlDoc = new XmlDocument();
    //    xmlDoc.Load(path_to_data_config);

    //    // Find the <Clock> element
    //    XmlNode? clockNode = xmlDoc.SelectSingleNode("//Clock");
    //    if (clockNode != null)
    //    {
    //        // Check if the <StartDate> value is null or empty
    //        if (string.IsNullOrEmpty(clockNode.InnerText))
    //        {
    //            setClock(DateTime.Now);
    //            return getClock();
    //        }
    //        else
    //        {
    //            // <Clock> has a value
    //            //return DateTime.TryParse(clockNode.InnerText);
    //            if (DateTime.TryParse(clockNode.InnerText, out DateTime result))
    //            {
    //                return result;
    //            }
    //            else
    //            {
    //                // Handle the situation where the date could not be parsed
    //                setClock(DateTime.Now);
    //                return getClock();
    //            }

    //        }
    //    }
    //    else
    //    {
    //        setClock(DateTime.Now);
    //        return getClock();
    //    }

    //}

    //#endregion

    #region clock
    public static void SetClock(DateTime clock)
    {
        // Load the XML document
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path_to_data_config);

        // Find the <Clock> element
        XmlNode clockNode = xmlDoc.SelectSingleNode("//Clock");
        if (clockNode != null)
        {
            // Update the <Clock> value to the new date
            clockNode.InnerText = clock.ToString("o"); // Use a standard date format

            // Attempt to save the changes back to the file
            xmlDoc.Save(path_to_data_config);
        }
        else
        {
            throw new BlDoesNotExistException("Clock node was not found in the XML.");
        }
    }

    public static DateTime GetClock()
    {
        // Load the XML document
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path_to_data_config);

        // Find the <Clock> element
        XmlNode clockNode = xmlDoc.SelectSingleNode("//Clock");
        if (clockNode != null)
        {
            // Try to parse the InnerText to a DateTime object
            if (DateTime.TryParse(clockNode.InnerText, null, System.Globalization.DateTimeStyles.RoundtripKind, out DateTime result))
            {
                return result;
            }
        }

        // If the <Clock> node is not found or the date could not be parsed, return DateTime.Now
        return DateTime.Now;
    }
    #endregion

    #endregion
}
