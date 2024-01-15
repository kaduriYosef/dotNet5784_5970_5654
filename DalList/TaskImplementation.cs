
namespace Dal;
using DalApi;
using DO;

/// <summary>
/// the class for the implemantation of ITask in the data source
/// </summary>
internal class TaskImplementation : ITask
{
    /// <summary>
    /// create new task (with new running id given)
    /// </summary>
    /// <param name="item">the task that needed to be created</param>
    /// <returns></returns>
    public int Create(Task item)
    {

        int id = DataSource.Config.NextTaskId;
        Task new_task = item with { Id = id };
        DataSource.Tasks.Add(new_task);
        return id;


    }


    /// <summary>
    /// read a task with the given id if such exist, if not returns null
    /// </summary>
    /// <param name="id">the id to find the task to read</param>
    /// <returns></returns>
    public Task? Read(int id)
    {
        return DataSource.Tasks.FirstOrDefault(result => result.Id == id);
    }

    /// <summary>
    /// read a task that satasfy the given predicat if such exist, if not returns null
    /// </summary>
    /// <param name="filter">the predicate to find the task by</param>
    /// <returns></returns>
    public Task? Read(Func<Task, bool> filter) // stage 2
    {
        if(filter == null) return null;
        return DataSource.Tasks.FirstOrDefault(task => filter(task));
    }

    /// <summary>
    /// give a list of all task that satisfy the predicate if given any, 
    /// if not just return all there is
    /// </summary>
    /// <param name="filter">the prdicate to filter the task by</param>
    /// <returns></returns>
    public IEnumerable<Task?> ReadAll(Func<Task, bool>? filter = null) // stage 2
    {
        if (filter == null)
            return DataSource.Tasks.Select(item => item);
        else
            return DataSource.Tasks.Where(filter);
    }

    /// <summary>
    /// updatet a task (recognized by its id) 
    /// if no task with such id exists throw exception
    /// </summary>
    /// <param name="item">the new item to update</param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Task item)
    {

        int index = DataSource.Tasks.FindIndex(e => e.Id == item.Id);

        if (index != -1)
        {
            DataSource.Tasks.RemoveAt(index);
            DataSource.Tasks.Add(item);
        }
        else
        {
            throw new DalDoesNotExistException($"A Task with id={item.Id} does not exist.\n");
        }
    }
    /// <summary>
    /// delete the task with the given id if such exists, if not throws exception
    /// </summary>
    /// <param name="id">the id of the task to delete</param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        //if such index exists we will remove this task
        //and if this index doesn't exist, meaning there isn't Task with such id, we will throw an exception
        int index = DataSource.Tasks.FindIndex((en) => en.Id == id);
        if (index != -1)
            DataSource.Tasks.RemoveAt(index);
        else 
            throw new DalDoesNotExistException($"there isn't an Task with Id={id}.\n");
    }
}
