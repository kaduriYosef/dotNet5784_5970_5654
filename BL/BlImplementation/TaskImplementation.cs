
namespace BlImplementation;

using BlApi;
using DO;
using System.Reflection.Metadata.Ecma335;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    private bool checkValidety(BO.Task boTask)
    {
        if(boTask == null) return false;
        if (boTask.Alias == "") return false;
        //if (boTask.Id < 0) return false;                  //completely unneccecary and useless
        return true;
    }
    public int Create(BO.Task boTask)
    {
        if(!checkValidety(boTask)) throw;

        foreach (var t in boTask.Dependencies)
            _dal.Dependency.Create(new DO.Dependency
                (
                 
                dependent: boTask.Id,
                dependsOn: t.Id
                ));

        int retId = _dal.Task.Create(BOtoDO(boTask));

        return retId;
    }




    public void Delete(int id)
    {
        
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask == null) throw new BlDoesNotExistException($"Task with Id = {id} doesn't exist");

        if(_dal.Dependency.ReadAll(dep=>dep.DependsOnTask==id).Any())
            throw 

        foreach(var  dependsOn in _dal.Dependency.ReadAll(dep=>dep.DependentTask==id))
        {
            _dal.Dependency.Delete(dependsOn!.Id);
        }

        _dal.Task.Delete(id); 

    }

    public BO.Task? Read(int id)
    {
        DO.Task doTask = _dal?.Task?.Read(id) ?? throw BlDoesNotExistException($"Task with Id= {id} doesn't exist.");

        return DOtoBO(doTask);
    }

    public BO.Task? Read(Func<BO.Task, bool> filter)
    {
        Func<DO.Task, bool> doFilter = t => filter(DOtoBO(t));
        DO.Task doTask =_dal?.Task?.Read(doFilter)?? BlDoesNotExistException($"Task that correspondes to such filter doesn't exist.");
        return DOtoBO(doTask);
    }

    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void StartTimeManagment(int id, DateTime date)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task item)
    {
        throw new NotImplementedException();
    }


    internal DO.Task BOtoDO(BO.Task boTask)
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
            Status = null,                                    //to calculate maybe
            Dependencies = (List<BO.TaskInList>)dependencies,
            Milestone = null,                                      //to calculate ,add of milestone
            RequiredEffortTime = doTask.RequiredEffortTime,
            StartDate = doTask.StartDate,
            ScheduledDate = doTask.ScheduledDate,
            DeadlineDate = doTask.DeadlineDate,
            CompleteDate = doTask.CompleteDate,
            Deliverables = doTask.Deliverables,
            Remarks = doTask.Remarks,
            Engineer = fromEngineerToEngineerInTask(_dal.Engineer.Read(doTask.EngineerId??-1)),
            Complexity=(BO.EngineerExperience)(int)doTask.Complexity
        };

        return boTask;
    }

    #region simplify tasks
    internal BO.TaskInList? fromTaskToTaskInList(DO.Task? doTask)
    {
        if (doTask is null) return null;
        return new BO.TaskInList
        {
            Id = doTask.Id,
            Alias = doTask.Alias,
            Description = doTask.Description
        };
    }
    internal BO.TaskInList? fromTaskToTaskInList(BO.Task? boTask)
    {
        if (boTask is null) return null;
        return new BO.TaskInList
        {
            Id = boTask.Id,
            Alias = boTask.Alias,
            Description = boTask.Description
        };
    }

    internal BO.TaskInEngineer? fromTaskToTaskInEngineer(DO.Task? doTask)
    {
        if (doTask is null) return null;
        return new BO.TaskInEngineer
        {
            Id = doTask.Id,
            Alias = doTask.Alias
        };


    }
    internal BO.TaskInEngineer? fromTaskToTaskInEngineer(BO.Task? boTask)
    {
        if (boTask is null) return null;
        return new BO.TaskInEngineer { 
            Id = boTask.Id, 
            Alias = boTask.Alias };
    
    
    }



    #endregion

}