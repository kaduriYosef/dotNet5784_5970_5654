using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;


using System.Data.Common;
using DalApi;
using DO;

internal class TaskImplementation:ITask
{
    readonly string s_Task_xml = "tasks";

    public int Create(Task item)
    {
        List<Task> Tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_Task_xml);
        int id = Config.NextTaskId;
        Task new_task = item with { Id = id };
        Tasks.Add(new_task);
        XMLTools.SaveListToXMLSerializer(Tasks, s_Task_xml);
        return id;
    }

    public void Delete(int id)
    {
        List<Task> Tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_Task_xml);
        int index = Tasks.FindIndex((en) => en.Id == id);
        if (index != -1)
            Tasks.RemoveAt(index);
        else
            throw new DalDoesNotExistException($"there isn't an Task with Id={id}.\n");
        XMLTools.SaveListToXMLSerializer(Tasks, s_Task_xml);

    }

    public Task? Read(int id)
    {
        List<Task> Tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_Task_xml);
        return Tasks.FirstOrDefault(result => result.Id == id);
    }

    public Task? Read(Func<Task, bool> filter)
    {
        List<Task> Tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_Task_xml);
        if (filter == null) return null;
        return Tasks.FirstOrDefault(task => filter(task));
    }

    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null)
    {
        List<Task> Tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_Task_xml);
        if (filter == null)
            return Tasks.Select(item => item);
        else
            return Tasks.Where(filter);
    }

    public void Update(Task item)
    {
        List<Task> Tasks = XMLTools.LoadListFromXMLSerializer<Task>(s_Task_xml);
        int index = Tasks.FindIndex(e => e.Id == item.Id);

        if (index != -1)
        {
            Tasks.RemoveAt(index);
            Tasks.Add(item);
        }
        else
        {
            throw new DalDoesNotExistException($"A Task with id={item.Id} does not exist.\n");
        }
        XMLTools.SaveListToXMLSerializer(Tasks, s_Task_xml);

    }
}
