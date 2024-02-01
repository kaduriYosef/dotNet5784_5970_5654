

namespace BlApi;


public interface ITask : ICrud<BO.Task>
{

    public void StartTimeManagment(int id,DateTime date);

}
