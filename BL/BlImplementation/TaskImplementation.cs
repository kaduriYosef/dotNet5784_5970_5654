
namespace BlImplementation;

using BlApi;
using BO;
using DO;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    private void checkValidity(BO.Task boTask)
    {
        if (boTask == null) throw new BO.BlInvalidDataException( "this BO.Task is null");
        string error = "";
        if (boTask.Alias == "") error+="Alias can't be empty. ";
        //if (boTask.Id < 0) error+="Id can't be less than zero.  ";                  //completely unneccecary and useless since the id is running
        if (boTask.RequiredEffortTime is not null && (boTask.RequiredEffortTime < TimeSpan.Zero))
            error += "required effort time can't be less than zero";

        
        if(error !="") throw new BO.BlInvalidDataException( error );
    }
    
    
    
    
    #region crud
    public int Create(BO.Task boTask)
    {
        checkValidity(boTask);
        foreach (var t in boTask.Dependencies)
            _dal.Dependency.Create(new DO.Dependency
                (
                dependent: boTask.Id,
                dependsOn: t.Id
                ));

        //nothing can go wrong after the checkValidity so 
        int retId = _dal.Task.Create(BOtoDO(boTask));

        return retId;
    }




    public void Delete(int id)
    {
        
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null) throw new BO.BlDoesNotExistException($"Task with Id = {id} doesn't exist");

        if (_dal.Dependency.ReadAll(dep => dep.DependsOnTask == id).Any())
            throw new BO.BlImpossibleToDeleteException("other tasks depends on this task");

        foreach(var  dependsOn in _dal.Dependency.ReadAll(dep=>dep.DependentTask==id))
        {
            _dal.Dependency.Delete(dependsOn!.Id);
        }

        _dal.Task.Delete(id); 

    }

    public BO.Task? Read(int id)
    {
        DO.Task doTask = _dal?.Task?.Read(id) ?? throw new BO.BlDoesNotExistException($"Task with Id= {id} doesn't exist.");

        return DOtoBO(doTask);
    }

    public BO.Task? Read(Func<BO.Task, bool> boFilter)
    {
        Func<DO.Task, bool> doFilter = t => boFilter(DOtoBO(t));
        DO.Task doTask =_dal?.Task?.Read(doFilter)?? throw new BO.BlDoesNotExistException($"Task that correspondes to such filter doesn't exist.");
        return DOtoBO(doTask);
    }

    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        Func<DO.Task, bool>? doFilter = null;
        if (filter is not null)
            doFilter= t => filter!(DOtoBO(t));

        return from t in _dal.Task.ReadAll(doFilter)
               select DOtoBO(t);
    }

    public IEnumerable<BO.TaskInList> ReadAllSimplified(Func<BO.Task, bool>? filter = null)
    {
        return from t in ReadAll(filter)
               select fromTaskToTaskInList(t);
    }

    public void StartTimeManagement(int id, DateTime date)
    {

        BO.Task boTask=Read(id)!;
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask is null)
            throw new BO.BlDoesNotExistException($"Task with Id = {id} doesn't exist");

        if (boTask!.Dependencies.Any(t => _dal.Task.Read(t.Id)?.StartDate is null))
            throw new BO.BlImpossibleToUpdateException
                ("can't declare start date this task before declaring start date for all of the tasks of which this one depends on.");

        if (boTask!.Dependencies.Any(t =>Read(t.Id)?.ForecastDate >date))
            throw new BO.BlImpossibleToUpdateException
                ("can't declare start date to be earlier than the Forecast date of the tasks of which this one depends on.");

        try
        {
            _dal.Task.Update(doTask with { StartDate = date });
        }
        catch(DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException(ex.Message);
        }
    }

    public void Update(BO.Task boTask)
    {
        checkValidity(boTask);
        
    }
#endregion
    #region convertors
    static internal DO.Task BOtoDO(BO.Task boTask)
    {
        return new DO.Task(
            Id: boTask.Id,
            Alias: boTask.Alias,
            Description: boTask.Description,
            CreatedAtDate: boTask.CreatedAtDate,
            RequiredEffortTime: boTask.RequiredEffortTime,
            IsMilestone: false,
            Complexity: (DO.EngineerExperience)(int)(boTask.Complexity),
            StartDate: boTask.StartDate,
            ScheduledDate: boTask.ScheduledDate,
            DeadlineDate: boTask.DeadlineDate,
            CompleteDate: boTask.CompleteDate,
            Deliverables: boTask.Deliverables,
            Remarks: boTask.Remarks,
            EngineerId: boTask.Engineer?.Id ?? null
            );
    }


    internal BO.Task DOtoBO(DO.Task doTask)
    {
        
        IEnumerable<BO.TaskInList> dependencies =
           (from dep in _dal.Dependency.ReadAll(dep=>dep.DependentTask==doTask.Id)
           select fromTaskToTaskInList(_dal.Task.Read(dep.DependsOnTask))).
           Where(x => x != null);


        BO.Task boTask = new BO.Task
        {
            Id = doTask.Id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = CalcStatus(doTask),
            Dependencies = (List<BO.TaskInList>)dependencies,
            Milestone = null,                                      //to calculate ,add of milestone
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            
            ForecastDate = (doTask.ScheduledDate is null || doTask.RequiredEffortTime is null )? null
            :doTask.ScheduledDate+doTask.RequiredEffortTime,

            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            
            //-1 can't be an ids
            Engineer = BlImplementation.EngineerImplementation.
            fromEngineerToEngineerInTask(_dal.Engineer.Read(doTask.EngineerId??-1)), 
            
            Complexity=(BO.EngineerExperience)(int)doTask.Complexity
        };

        return boTask;
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
            Status =  CalcStatus(doTask)
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
            Status= boTask.Status
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
        return new BO.TaskInEngineer { 
            Id = boTask.Id, 
            Alias = boTask.Alias };
    
    
    }



    #endregion

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


}