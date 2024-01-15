
namespace Dal;
using DalApi;
using DO;
/// <summary>
/// the class for the implemantation of IDependency in the data source
/// </summary>
internal class DependencyImplementation : IDependency
{
    /// <summary>
    /// create new dependency (with new running id given)
    /// </summary>
    /// <param name="item">the dependenct that needed to be created</param>
    /// <returns></returns>
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency new_item = item with { Id = id };
        DataSource.Dependencies.Add(new_item);
        return id;

    }

    /// <summary>
    /// read a dependency with the given id if such exist, if not returns null
    /// </summary>
    /// <param name="id">the id to find the dependency to read</param>
    /// <returns></returns>
    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(dep => dep.Id == id);
    }
    /// <summary>
    /// read a dependency that satasfi the given predicat if such exist, if not returns null
    /// </summary>
    /// <param name="filter">the predicate to find the dependency by</param>
    /// <returns></returns>
    public Dependency? Read(Func<Dependency, bool> filter) // stage 2
    {
        if (filter == null) return null;
        return DataSource.Dependencies.FirstOrDefault(dep => filter(dep));
    }
    /// <summary>
    /// give a list of all dependencies that satisfy the predicate if given any, 
    /// if not just return all there is
    /// </summary>
    /// <param name="filter">the prdicate to filter the dependencies by</param>
    /// <returns></returns>
    public IEnumerable<Dependency?> ReadAll(Func<Dependency, bool>? filter = null) // stage 2
    {
        if (filter == null)
            return DataSource.Dependencies.Select(item => item);
        else
            return DataSource.Dependencies.Where(filter);
    }
    /// <summary>
    /// updatet a dependency (recognized by its id) 
    /// if no dependency with such id exists throw exception
    /// </summary>
    /// <param name="item">the new item to update</param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Dependency item)
    // find the index of the item to update if it exist. if not throws an exeption
    {
        int index = DataSource.Dependencies.FindIndex(dep => dep.Id == item.Id);

        if (index != -1)
        {
            DataSource.Dependencies.RemoveAt(index);
            DataSource.Dependencies.Add(item);
        }
        else
        {
            throw new DalDoesNotExistException($"An Dependency with id={item.Id} does not exist.\n");
        }
    }
    /// <summary>
    /// delete the dependency with the given id if such exists, if not throws exception
    /// </summary>
    /// <param name="id">the id of the dependency to delete</param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Delete(int id)
    {
        //if such index exists we will remove this Dependency
        //and if this index doesn't exist, meaning there isn't Dependency with such id, we will throw an exception
        int index = DataSource.Dependencies.FindIndex((en) => en.Id == id);
        if (index != -1)
            DataSource.Dependencies.RemoveAt(index);
        else
            throw new DalDoesNotExistException($"there isn't an Dependency with Id={id}.\n");
    }
    /// <summary>
    /// return true or false based on the existance of a dependency between to given tasks
    /// (tasks given by their id's)
    /// </summary>
    /// <param name="dependent_id"></param>
    /// <param name="dependsOn_id"></param>
    /// <returns></returns>
    public bool DoesExist(int dependent_id,int dependsOn_id)
    {
        return DataSource.Dependencies.Any(dep=>dep.DependentTask == dependent_id && dep.DependsOnTask==dep.DependsOnTask);
    }
}
