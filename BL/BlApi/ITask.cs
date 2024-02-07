

namespace BlApi;


public interface ITask : ICrud<BO.Task>
{
    public IEnumerable<BO.TaskInList> ReadAllSimplified(Func<BO.Task,bool>?filter=null);
    public void ScheduledDateManagement(int id,DateTime date);
    public void ScheduleAllDates(DateTime startOfProject);

}
