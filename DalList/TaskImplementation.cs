
namespace Dal;
using DalApi;
using DO;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {

        int id=DataSource.Config.NextTaskId;
        Task new_task = item with { Id=id };
        DataSource.Tasks.Add( new_task);
        return id;
        

    }


    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(result => result.Id == id);
    }

    public List<Task> ReadAll()
    {
        return new List<Task>(DataSource.Tasks);
    }

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
            throw new Exception($"A Task with id={item.Id} does not exist.\n");
        }
    }
    public void Delete(int id)
    {
        //if such index exists we will remove this task
        //and if this index doesn't exist, meaning there isn't Task with such id, we will throw an exception
        int index = DataSource.Tasks.FindIndex((en) => en.Id == id);
        if (index != -1)
            DataSource.Tasks.RemoveAt(index);
        else 
            throw new Exception($"there isn't an Task with Id={id}.\n");
    }
}
