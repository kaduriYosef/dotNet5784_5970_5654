

namespace BlApi;


public interface ITask : ICrud<Task>
{

    public void StartTimeManagment(int id,DateTime date);

}
