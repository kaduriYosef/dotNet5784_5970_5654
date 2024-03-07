
namespace BlImplementation;

using BlApi;
using BO;
using DO;
using System.Collections.Immutable;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Linq;

/// <summary>
/// Implements the ITask interface, providing business logic and operations for task management.
/// </summary>
internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;


    /// <summary>
    /// Checks the validity of a BO.Task object, throwing exceptions if invalid data is found.
    /// </summary>
    /// <param name="boTask">The BO.Task object to validate.</param>

    static readonly BlApi.IBl e_bl = BlApi.Factory.Get();
    private void checkValidity(BO.Task boTask)
    {
        if (boTask == null) throw new BO.BlInvalidDataException( "this BO.Task is null");
        string error = "";
        if (boTask.Alias == "") error+="Alias can't be empty. ";
        //if (boTask.Id < 0) error+="Id can't be less than zero. ";                  //completely unnecessary and useless since the id is running
        if (boTask.RequiredEffortTime is not null && (boTask.RequiredEffortTime < TimeSpan.Zero))
            error += "required effort time can't be less than zero";

        BO.Engineer checkLevel;
        if (boTask.Engineer?.Id is not null)
        {
            checkLevel = e_bl.Engineer.Read(boTask.Engineer?.Id ?? 0)!;
            if (boTask.Complexity > checkLevel.Level)
            {
                boTask.Engineer = null;
                throw new BO.BlInvalidDataException("This task requires a higher level of engineer");
            }
            if (error != "") throw new BO.BlInvalidDataException(error);
        }

    }



    #region crud

    /// <summary>
    /// Creates a new task in the data layer and manages dependencies.
    /// </summary>
    /// <param name="boTask">The task to create.</param>
    /// <returns>The ID of the newly created task.</returns>
    public int Create(BO.Task boTask)
    {
        if (Tools.StartDateOrNull() != null)
            throw new BlImpossibleToCreate("can't create new task once the start date was declared");
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


    /// <summary>
    /// Reads a task based on its ID.
    /// </summary>
    /// <param name="id">The ID of the task to read.</param>
    /// <returns>The task associated with the given ID.</returns>

    public BO.Task? Read(int id)
    {
        DO.Task doTask = _dal?.Task?.Read(id) ?? throw new BO.BlDoesNotExistException($"Task with Id= {id} doesn't exist.");

        return DOtoBO(doTask);
    }



    /// <summary>
    /// Reads a task based on a provided filter function.
    /// </summary>
    /// <param name="boFilter">The filter function to apply.</param>
    /// <returns>The task that matches the filter criteria.</returns>

    public BO.Task? Read(Func<BO.Task, bool> boFilter)
    {
        Func<DO.Task, bool> doFilter = t => boFilter(DOtoBO(t));
        DO.Task doTask =_dal?.Task?.Read(doFilter)?? throw new BO.BlDoesNotExistException($"Task that correspondes to such filter doesn't exist.");
        return DOtoBO(doTask);
    }


    /// <summary>
    /// Reads all tasks, optionally filtered by a provided function.
    /// </summary>
    /// <param name="filter">The optional filter function to apply.</param>
    /// <returns>An enumerable of all (filtered) tasks.</returns>

    public IEnumerable<BO.Task?> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        Func<DO.Task, bool>? doFilter = null;
        if (filter is not null)
            doFilter= t => filter!(DOtoBO(t));

        return from t in _dal.Task.ReadAll(doFilter)
               select DOtoBO(t);
    }

    /// <summary>
    /// Reads all tasks in a simplified format, optionally applying a filter.
    /// </summary>
    /// <param name="filter">The optional filter function to apply.</param>
    /// <returns>An enumerable of all (filtered) tasks in a simplified format.</returns>

    public IEnumerable<BO.TaskInList> ReadAllSimplified(Func<BO.Task, bool>? filter = null)
    {
        return from t in ReadAll(filter)
               select Tools.fromTaskToTaskInList(t);
    }


    /// <summary>
    /// Manages the scheduled date for a task, ensuring it meets dependency requirements.
    /// </summary>
    /// <param name="id">The ID of the task to manage.</param>
    /// <param name="date">The scheduled date to set for the task.</param>

    public void ScheduledDateManagement(int id, DateTime date)
    {

        BO.Task boTask=Read(id)!;
        DO.Task? doTask = _dal.Task.Read(id);
        if (doTask is null)
            throw new BO.BlDoesNotExistException($"Task with Id = {id} doesn't exist");

        if (boTask!.Dependencies.Any(t => _dal.Task.Read(t.Id)?.ScheduledDate is null))
            throw new BO.BlImpossibleToUpdateException
                ("can't declare start date this task before declaring start date for all of the tasks of which this one depends on.");


        var dates = from t in boTask.Dependencies
                    let task = _dal.Task.Read(t.Id)
                    where task is not null
                    let start = task.StartDate
                    let scheduled = task.ScheduledDate
                    let maxStart =(start is null) ? scheduled 
                        : ((start > scheduled) ? start : scheduled)
                    select maxStart + task.RequiredEffortTime;



        if (dates.Any(da => da > date))
            throw new BO.BlImpossibleToUpdateException
                ("can't declare start date to be earlier than the Forecast date of the tasks of which this one depends on.");

        try
        {
            _dal.Task.Update(doTask with { ScheduledDate = date });
        }
        catch(DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException(ex.Message);
        }
    }


    /// <summary>
    /// Unsafe version of ScheduledDateManagement, directly setting the scheduled date without dependency checks.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="date">The date to set as the scheduled date.</param>

    protected void ScheduledDateManagementUnsafe(int id, DateTime date)
    {
        var doTask = _dal.Task.Read(id);
        if (doTask == null)
            throw new BO.BlDoesNotExistException($"task with id {id} doesn't exist. ");

        _dal.Task.Update(doTask with { ScheduledDate = date });
    }


    /// <summary>
    /// Updates a task, handling dependencies and validation.
    /// </summary>
    /// <param name="boTask">The task to update.</param>
    public void Update(BO.Task boTask)
    {
        var boTaskOriginal = Read(boTask.Id);
        if (boTaskOriginal == null)
            throw new BlDoesNotExistException($"task with id {boTask.Id} doesn't exist");

        if (Tools.StartDateOrNull() != null && 
            (boTask.ScheduledDate!=boTaskOriginal.ScheduledDate ||boTask.RequiredEffortTime!=boTaskOriginal.RequiredEffortTime|| boTask.Dependencies!=boTaskOriginal.Dependencies))
                throw new BlImpossibleToCreate("can't update new task once the start date was declared");


        checkValidity(boTask);

        //delete all previous dependencies
        foreach (var depOfBoTask in _dal.Dependency.ReadAll(dep => dep.DependentTask == boTask.Id))
            if (depOfBoTask is not null) _dal.Dependency.Delete(depOfBoTask.Id);

        //create new dependencies 
        foreach (var id in boTask.Dependencies.Select(boTinEng => boTinEng.Id))
            _dal.Dependency.Create(new DO.Dependency(dependent: boTask.Id, dependsOn: id));

        

        try
        {
            _dal.Task.Update(BOtoDO(boTask));
        }
        catch(DO.DalDoesNotExistException ex) 
        { throw new BO.BlDoesNotExistException(ex.Message); }
        
        ////need just for the checking of the validity of the date
        //if(boTask.StartDate is not null)
        //    ScheduledDateManagement(boTask.Id, (DateTime)boTask.StartDate!);
    }

    /// <summary>
    /// Deletes a task and its dependencies based on the task ID.
    /// </summary>
    /// <param name="id">The ID of the task to delete.</param>
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

    #endregion

    #region convertors

    /// <summary>
    /// Converts a BO.Task object to a DO.Task object.
    /// </summary>
    /// <param name="boTask">The BO.Task to convert.</param>
    /// <returns>The converted DO.Task object.</returns>
    static internal DO.Task BOtoDO(BO.Task boTask)
    {
        return new DO.Task(
            Id: boTask.Id,
            Alias: boTask.Alias,
            Description: boTask.Description,
            CreatedAtDate: boTask.CreatedAtDate,
            RequiredEffortTime: boTask.RequiredEffortTime,
            IsMilestone: false,
            Complexity: (DO.EngineerExperience?)(int?)(boTask.Complexity),
            StartDate: boTask.StartDate,
            ScheduledDate: boTask.ScheduledDate,
            DeadlineDate: boTask.DeadlineDate,
            CompleteDate: boTask.CompleteDate,
            Deliverables: boTask.Deliverables,
            Remarks: boTask.Remarks,
            EngineerId: boTask.Engineer?.Id ?? null
            );
    }


    /// <summary>
    /// Converts a DO.Task object to a BO.Task object.
    /// </summary>
    /// <param name="doTask">The DO.Task to convert.</param>
    /// <returns>The converted BO.Task object.</returns>
    internal BO.Task DOtoBO(DO.Task doTask)
    {
        
        IEnumerable<BO.TaskInList> dependencies =
           (from dep in _dal.Dependency.ReadAll(dep=>dep.DependentTask==doTask.Id)
           select Tools.fromTaskToTaskInList(_dal.Task.Read(dep.DependsOnTask))).
           Where(x => x != null);

        BO.Task boTask = new BO.Task
        {
            Id = doTask.Id,
            Alias = doTask.Alias,
            Description = doTask.Description,
            CreatedAtDate = doTask.CreatedAtDate,
            Status = Tools.CalcStatus(doTask),
            Dependencies = dependencies.ToList(),
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
            Engineer = Tools.
            fromEngineerToEngineerInTask(_dal.Engineer.Read(doTask.EngineerId??-1)), 
            
            Complexity=(doTask.Complexity==null)?null:(BO.EngineerExperience)(int)doTask.Complexity
        };

        return boTask;
    }


    static internal BO.Task DOtoBO(DalApi.IDal dal, DO.Task doTask)
    {
        TaskImplementation tmp
            = new TaskImplementation { _dal= dal};
        return tmp.DOtoBO(doTask);
    }

    #endregion


    #region schedule



    ////external function  to reset all the ScheduledDate and the deadline of all the task 
    //public void ScheduleAllDates(DateTime startProject)
    //{
    //    //find all the tasks that do not depend on any other tasks 
    //    IEnumerable<BO.Task> notDependentTask = from doTask in _dal.Task.ReadAll()
    //                                            let boTask = Read(doTask.Id)
    //                                            where boTask.Dependencies == null || boTask.Dependencies.Count() == 0
    //                                            select boTask;
    //    reset(startProject, notDependentTask);
    //    Console.WriteLine("i'm here\n");
    //}
    ////recursive function, reset all the ScheduledDate and the deadline of all the task
    //public void reset(DateTime? prevDate, IEnumerable<BO.Task>? tasks)
    //{
    //    if (tasks != null && tasks.Count() != 0)
    //        foreach (var item in tasks)
    //        {
    //            //update the task whit the correct ScheduledDate and DeadlineDate
    //            _dal.Task.Update(BOtoDO(item) with
    //            {
    //                ScheduledDate = (item.ScheduledDate == null || item.ScheduledDate < prevDate)
    //                ? prevDate
    //                : item.ScheduledDate,
    //                //ForecastDate = prevDate + item.RequiredEffortTime
    //            });
    //            //sending the tasks that is depending on this task
    //            reset(prevDate + item.RequiredEffortTime, from dep in _dal.Dependency.ReadAll()
    //                                                      where dep.DependsOnTask == item.Id
    //                                                      select Read(dep.DependentTask));
    //        }
    //}


    /// <summary>
    /// Schedules all dates for tasks based on the start of the project.
    /// </summary>
    /// <param name = "startOfProject" > The start date of the project.</param>

    public void ScheduleAllDates(DateTime startOfProject)
    {
        if (startOfProject < DateTime.Now)
            throw new BlImpossibleToUpdateException( "can't select a past date");

        // Define the path to your XML file
        DateTime? tmp=Tools.StartDateOrNull();
        if (tmp != null)
            throw new BlImpossibleToUpdateException("project already has a start date!");
        
        Tools.update_StartDate_unsafe(startOfProject);
        foreach (var boTask in ReadAll().Where(x=>x is not null))
        {
            TimeSpan required = boTask!.RequiredEffortTime ?? TimeSpan.FromDays(10);
            _dal.Task.Update(BOtoDO(boTask)with { ScheduledDate =null,RequiredEffortTime=required});
        }
        //IEnumerable<BO.TaskInList> boTasks = ReadAllSimplified();
        //find all the tasks without dependencies
        List<BO.Task?> tasksWithoutDependency =
            ReadAll().Where(x => x is not null).
            Where(t =>t!.Dependencies==null || !(t!.Dependencies.Any())).
            ToList();

        //    Enter a start and end date
        foreach (var task in tasksWithoutDependency)
        {
            //store the dates in the data layer
            ScheduledDateManagementUnsafe(task!.Id, startOfProject);
        }

        //Finds all the tasks who depends on something
        List<BO.Task?> tasksWithDependency =
          ReadAll().Where(x => x is not null).
            Where(t => (t!.Dependencies.Any())).
            ToList();

        foreach (var task in tasksWithDependency)
        {
            initScheduledDateRecursive(task!);
        }

    }


    //recursive supporting function
    private DateTime? initScheduledDateRecursive(BO.Task task)
    {
        if (task.ForecastDate != null)
            return task.ForecastDate;

        DateTime? ForecastDateFromDepend = DateTime.MinValue;
        foreach (var depTask in task.Dependencies)
        {
            var fullDepTask = Read(depTask.Id);
            if (fullDepTask is null) continue;

            DateTime? tmp = initScheduledDateRecursive(fullDepTask);
            if (tmp!=null && ForecastDateFromDepend < tmp)
                ForecastDateFromDepend = tmp;

        }
        if (task.ScheduledDate == null)
        {
            ScheduledDateManagementUnsafe(task.Id, ForecastDateFromDepend.GetValueOrDefault());
            task.ScheduledDate = ForecastDateFromDepend;
            task.ForecastDate = ForecastDateFromDepend + task.RequiredEffortTime;
        }
        else
        {
            task.ScheduledDate =
                
                (task.ScheduledDate > ForecastDateFromDepend) ? task.ScheduledDate : ForecastDateFromDepend;

            ScheduledDateManagementUnsafe(task.Id, task.ScheduledDate.GetValueOrDefault());
            task.ForecastDate = task.ScheduledDate + task.RequiredEffortTime;
        }

        return task.ForecastDate;
    }


    #endregion
   
}