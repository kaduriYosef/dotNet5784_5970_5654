namespace BlImplementation;

using System.Data.Common;
using System.Security.Cryptography;
using System.Net.Mail;
using BlApi;
using BO;
using DO;

/// <summary>
/// 
/// </summary>
internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="boEngineer"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlAlreadyExistException"></exception>
    public int Create(BO.Engineer boEngineer)
    {
        checkValidity(boEngineer);

        DO.Engineer doEngineer = BOEngineerToDOEngineer(boEngineer);
        try
        {
            int idEngineer = _dal.Engineer.Create(doEngineer);
            return idEngineer;
        }
        catch (DO.DalAlreadyExistsException ex) 
        {
            throw new BO.BlAlreadyExistException($"Engineer with ID={boEngineer.Id} already exist", ex);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="BO.BlAlreadyExistException"></exception>
    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);

        if(doEngineer == null)
            throw new BO.BlAlreadyExistException($"Engineer with ID={id} doesn't exist");

        return DOEngineerToBOEngineer(doEngineer);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public BO.Engineer? Read(Func<BO.Engineer, bool> filter)
    {
        if (filter == null) return null;
        return ReadAll().FirstOrDefault(eng => filter(eng));
    }

    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                select DOEngineerToBOEngineer (doEngineer));
                
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="BOEngineer"></param>
    /// <exception cref="BO.BlDoesNotExistException"></exception>
    public void Update(BO.Engineer BOEngineer)
    {
        DO.Engineer? eng = _dal.Engineer.Read(BOEngineer.Id);
        if (eng == null) throw new BO.BlDoesNotExistException($"the engineer with Id: {BOEngineer.Id} isn't exist");

        checkValidity(BOEngineer);

        var doTasks = _dal.Task.ReadAll(task => task.EngineerId == BOEngineer.Id);

        foreach (var t in doTasks.Where(t => t is not null))
        {
            var tNew = t! with { EngineerId = null };
            _dal.Task.Update(tNew);
        }
        if (BOEngineer.Tasks is not null)
            foreach (var t in BOEngineer.Tasks.Where(t => t is not null))
            { var task =_dal.Task.Read(t.Id)! with { EngineerId=BOEngineer.Id};
                _dal.Task.Update(task);
                     }
        eng = BOEngineerToDOEngineer(BOEngineer);


        
      
        
            
    }

    public void Delete(int id)
    {
        try 
        {  
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BlDoesNotExistException(ex.Message);
        }
        
    }


#region convertors

    internal BO.Engineer DOEngineerToBOEngineer(DO.Engineer DOEngineer)
    {
        var tasks= _dal.Task.ReadAll().Where(x => x is not null).Where(task => task!.EngineerId == DOEngineer.Id);
        var task=_dal.Task.ReadAll().Where(x=>x is not null).FirstOrDefault(task => task!.EngineerId == DOEngineer.Id);
       return new BO.Engineer()
        {
            Id = DOEngineer.Id,
            Email = DOEngineer.Email,
            Cost = DOEngineer.Cost,
            Name = DOEngineer.Name,
            Level = (BO.EngineerExperience)(int)DOEngineer.Level,
            Task= Tools.fromTaskToTaskInEngineer(task),
            Tasks=(List<TaskInEngineer>)(from t in tasks select Tools.fromTaskToTaskInEngineer(t))
        };
    }

    internal DO.Engineer BOEngineerToDOEngineer(BO.Engineer BOEngineer)
    {
        if (BOEngineer.Task != null)
        {
            var task = _dal.Task.Read(BOEngineer.Task.Id);
            if (task == null)
                throw new BO.BlDoesNotExistException($"task with {BOEngineer.Task.Id} doesn't exit. ");
            _dal.Task.Update(task with { EngineerId=BOEngineer.Id});
        }
        return new DO.Engineer() { 
            Id = BOEngineer.Id,
            Email= BOEngineer.Email,
            Cost = BOEngineer.Cost,
            Name = BOEngineer.Name,
            Level= (DO.EngineerExperience)(int)BOEngineer.Level
        };
    }


    #endregion
    internal void checkValidity(BO.Engineer boEngineer)
    {
        if (boEngineer.Id <= 0) throw new BlInvalidDataException($"the engineer with Id: {boEngineer.Id} is invalid");
        if (boEngineer.Name == "") throw new BlInvalidDataException($"the engineer with Name: {boEngineer.Name} is invalid");
        //MailAddress check = new MailAddress(boEngineer.Email); 
        if (boEngineer.Email == "") throw new BlInvalidDataException($"the engineer with Email: {boEngineer.Email} is invalid");
        if (boEngineer.Cost <=0) throw new BlInvalidDataException($"the engineer with Cost: {boEngineer.Cost} is invalid");

        if (boEngineer.Task == null)
            return;

        DO.Task? doTask=_dal.Task.Read(boEngineer.Task.Id);
        if (doTask == null)
            throw new BO.BlInvalidDataException($"Task with id ={boEngineer.Task.Id} doesn't exist. ");

        bool areThereAnyIncompleteDeps =
            _dal.Dependency.ReadAll(dep => dep is not null && dep.DependentTask == doTask.Id).
            Any(dep =>
            (_dal.Task.Read(dep!.DependsOnTask) == null)
            ? throw new BO.BlNullPropertyException($"a dependency with id {dep.Id} contains an invalid id")
            : _dal.Task.Read(dep.DependsOnTask)!.CompleteDate == null);

        #region maybe to do
        //var dependsOn = _dal.Dependency.ReadAll(dep => dep is not null && dep.DependentTask == doTask.Id).
        //    GroupBy(dep =>
        //    (_dal.Task.Read(dep!.DependsOnTask) == null)
        //    ? throw new BO.BlNullPropertyException($"a dependency with id {dep.Id} contains an invalid id")
        //    : _dal.Task.Read(dep!.DependsOnTask)!.CompleteDate).
        //    Where(x => x is not null).
        //    FirstOrDefault(li =>
        //     _dal.Task.Read(li.FirstOrDefault(dep => true)!.DependsOnTask)!.CompleteDate == null);
        #endregion

        if (areThereAnyIncompleteDeps) 
            throw new BlInvalidDataException($"can't give the task with id {doTask.Id} to an engineer before all its previous tasks will be completed");

    }
}
