

namespace BlApi;


public interface ITask : ICrud<BO.Task>
{
    public IEnumerable<BO.TaskInList> ReadAllSimplified(Func<BO.Task,bool>?filter=null);
    public void StartTimeManagement(int id,DateTime date);

}
