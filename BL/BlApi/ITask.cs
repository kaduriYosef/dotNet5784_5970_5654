

namespace BlApi;


public interface ITask
{
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool> filter=null);

    public BO.Task? Read(int id); //will throw error if not found

    public int Creat(BO.Task itemBoTask);

    public void Update(BO.Task itemBoTask);

    public void Delete(BO.Task itemToDeleteBoTask);

    public void Update(int id,DateTime date);

}
