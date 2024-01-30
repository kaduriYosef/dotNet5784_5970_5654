

namespace BlApi;


public interface ITask
{
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool> filter=null);

    public BO.Task Read(int id); //will throw error if not found



}
