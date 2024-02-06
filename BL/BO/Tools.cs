
using System.ComponentModel;
using System.Text;

namespace BO;

static internal class Tools
{
    #region to string property
    static private string getNtabs(int n) 
    {
        string str = "";
        for (int i = 0;i<n;++i)
            str += "\t";
        return str;
            }
    public static string ToStringProperty<T>(this T obj,int tabs=0)
    {
        
        if (obj == null) return "null";
        var type = obj.GetType();
        var sb = new StringBuilder();

        // פתיחת סוגריים לציון התחלת העצם
        sb.Append($"{type.Name} \n{getNtabs(tabs)}{{\n ");

        foreach (var prop in type.GetProperties())
        {
            var value = prop.GetValue(obj);
            string valueString="";

            if (value is System.Collections.IEnumerable && !(value is string))
            {
                valueString += getNtabs(tabs);
                valueString += "\n[\n";
                foreach (var item in (value as System.Collections.IEnumerable)!)
                {
                    valueString += getNtabs(tabs);
                    valueString += ToStringProperty(item,tabs+1) + ", \n";
                }
                if (valueString.EndsWith(", \n")) 
                    valueString = valueString.Substring(0, valueString.Length - 3);
                valueString += getNtabs(tabs);
                valueString += "\n]\n";
            }
            else
            {
                valueString +=getNtabs(tabs);
                valueString += ToStringProperty(value,tabs+1);
            }

            sb.Append($"{prop.Name}: {valueString}, \n");
        }

        if (sb.ToString().EndsWith(", \n")) sb.Length -= 3; // ניקוי פסיק ורווח מיותרים בסוף

        // סגירת סוגריים לציון סיום העצם
        sb.Append(" \n}\n");

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
