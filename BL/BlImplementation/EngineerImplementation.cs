namespace BlImplementation;

using System.Data.Common;
using System.Security.Cryptography;
using System.Net.Mail;
using BlApi;
using BO;
using DO;

/// <summary>
/// Provides the implementation of the IEngineer interface, allowing interaction with engineer entities.
/// </summary>
internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    /// <summary>
    /// Creates a new engineer in the system.
    /// </summary>
    /// <param name="boEngineer">The business object representation of the engineer to be created.</param>
    /// <returns>The ID of the newly created engineer.</returns>
    /// <exception cref="BO.BlAlreadyExistException">Thrown when an engineer with the same ID already exists in the system.</exception>
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
    /// Reads and returns an engineer based on the given ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to read.</param>
    /// <returns>The engineer corresponding to the ID, if found.</returns>
    /// <exception cref="BO.BlAlreadyExistException">Thrown when no engineer with the given ID exists.</exception>
    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);

        if (doEngineer == null)
            throw new BO.BlAlreadyExistException($"Engineer with ID={id} doesn't exist");

        return DOEngineerToBOEngineer(doEngineer);

    }
    /// <summary>
    /// Reads and returns an engineer that matches the provided filter.
    /// </summary>
    /// <param name="filter">A function to filter engineers.</param>
    /// <returns>The first engineer that matches the filter, if any; otherwise, null.</returns>
    public BO.Engineer? Read(Func<BO.Engineer, bool> filter)
    {
        if (filter == null) return null;
        return ReadAll().FirstOrDefault(eng => filter(eng));
    }

    /// <summary>
    /// Reads all engineers that match the provided filter or all engineers if no filter is provided.
    /// </summary>
    /// <param name="filter">An optional filter to apply on the engineers.</param>
    /// <returns>An enumerable of all engineers that match the filter, or all engineers if no filter is provided.</returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        IEnumerable<BO.Engineer> readAll = (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                                            select DOEngineerToBOEngineer(doEngineer));
        if (filter == null)
            return readAll;
        return readAll.Where(filter);
    }
    /// <summary>
    /// Updates the information of an existing engineer.
    /// </summary>
    /// <param name="BOEngineer">The engineer with updated information.</param>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the engineer to update does not exist.</exception>
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
        if (BOEngineer.AdditionalTasks is not null)
            foreach (var t in BOEngineer.AdditionalTasks.Where(t => t is not null))
            {
                var task = _dal.Task.Read(t.Id)! with { EngineerId = BOEngineer.Id };
                _dal.Task.Update(task);
            }
        eng = BOEngineerToDOEngineer(BOEngineer);
    }

    /// <summary>
    /// Deletes an engineer based on the given ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to delete.</param>
    /// <exception cref="BO.BlDoesNotExistException">Thrown if the engineer to delete does not exist.</exception>
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

    /// <summary>
    /// Converts a DO.Engineer object to a BO.Engineer object.
    /// </summary>
    /// <param name="DOEngineer">The data object engineer to be converted.</param>
    /// <returns>A business object representation of the engineer.</returns>
    internal BO.Engineer DOEngineerToBOEngineer(DO.Engineer DOEngineer)
    {
        var task = _dal.Task.ReadAll().Where(x => x is not null).FirstOrDefault(task => task!.EngineerId == DOEngineer.Id);
        return new BO.Engineer()
        {
            Id = DOEngineer.Id,
            Email = DOEngineer.Email,
            Cost = DOEngineer.Cost,
            Name = DOEngineer.Name,
            Level = (BO.EngineerExperience)(int)DOEngineer.Level,
            Task = Tools.fromTaskToTaskInEngineer(task),
            AdditionalTasks = null
        };
    }

    /// <summary>
    /// Converts a BO.Engineer object to a DO.Engineer object.
    /// </summary>
    /// <param name="BOEngineer">The business object engineer to be converted.</param>
    /// <returns>A data object representation of the engineer.</returns>
    internal DO.Engineer BOEngineerToDOEngineer(BO.Engineer BOEngineer)
    {
        if (BOEngineer.Task != null)
        {
            var task = _dal.Task.Read(BOEngineer.Task.Id);
            if (task == null)
                throw new BO.BlDoesNotExistException($"task with {BOEngineer.Task.Id} doesn't exit. ");
            _dal.Task.Update(task with { EngineerId = BOEngineer.Id });
        }
        return new DO.Engineer()
        {
            Id = BOEngineer.Id,
            Email = BOEngineer.Email,
            Cost = BOEngineer.Cost,
            Name = BOEngineer.Name,
            Level = (DO.EngineerExperience)(int)BOEngineer.Level
        };
    }


    #endregion
    /// <summary>
    /// Checks the validity of the provided BO.Engineer object.
    /// </summary>
    /// <param name="boEngineer">The engineer to validate.</param>
    /// <exception cref="BlInvalidDataException">Thrown if any of the engineer's data is found to be invalid.</exception>
    internal void checkValidity(BO.Engineer boEngineer)
    {
        if (boEngineer.Id <= 0) throw new BlInvalidDataException($"the engineer with Id: {boEngineer.Id} is invalid");
        if (boEngineer.Name == "") throw new BlInvalidDataException($"the engineer with Name: {boEngineer.Name} is invalid");
        if (boEngineer.Email == "") throw new BlInvalidDataException($"the engineer with Email: {boEngineer.Email} is invalid");
        if (boEngineer.Cost <= 0) throw new BlInvalidDataException($"the engineer with Cost: {boEngineer.Cost} is invalid");

        if (boEngineer.Task == null)
            return;

        DO.Task? doTask = _dal.Task.Read(boEngineer.Task.Id);
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
