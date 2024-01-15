
namespace Dal;
using DalApi;
using DO;
using System.Reflection.Metadata.Ecma335;

/// <summary>
/// the class for the implemantation of IEngineer in the data source
/// </summary>
internal class EngineerImplementation : IEngineer
{
    /// <summary>
    /// create new engineer (with new running id given)
    /// </summary>
    /// <param name="item">the engineer that needed to be created</param>
    /// <returns></returns>
    public int Create(Engineer item)
    {
        bool isNew = !DataSource.Engineers.Any(en => en.Id == item.Id);
        if (isNew) DataSource.Engineers.Add(item);
        return isNew ? item.Id : throw new DalAlreadyExistsException($"Engineer with Id={item.Id} already exist.\n");
    }


    /// <summary>
    /// read a engineer with the given id if such exist, if not returns null
    /// </summary>
    /// <param name="id">the id to find the engineer to read</param>
    /// <returns></returns>
    public Engineer? Read(int id)
    {
       
        return DataSource.Engineers.FirstOrDefault(eng => eng.Id == id);
    }

    /// <summary>
    /// read a engineer that satasfy the given predicat if such exist, if not returns null
    /// </summary>
    /// <param name="filter">the predicate to find the engineer by</param>
    /// <returns></returns>
    public Engineer? Read(Func<Engineer, bool> filter) // stage 2
    {
        if (filter == null) return null;
        return DataSource.Engineers.FirstOrDefault(eng => filter(eng));
    }

    /// <summary>
    /// give a list of all engineer that satisfy the predicate if given any, 
    /// if not just return all there is
    /// </summary>
    /// <param name="filter">the prdicate to filter the engineer by</param>
    /// <returns></returns>
    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null) // stage 2
    {
        if (filter == null)
            return DataSource.Engineers.Select(item => item);
        else
            return DataSource.Engineers.Where(filter);
    }


    /// <summary>
    /// updatet a engineer (recognized by its id) 
    /// if no engineer with such id exists throw exception
    /// </summary>
    /// <param name="item">the new item to update</param>
    /// <exception cref="DalDoesNotExistException"></exception>
    public void Update(Engineer item)
    // find the index of the item to update if it exist. if not throws an exeption
    {
        int index = DataSource.Engineers.FindIndex(en => en.Id == item.Id);

        if (index != -1)
        {
            DataSource.Engineers.RemoveAt(index);
            DataSource.Engineers.Add(item);
        }
        else
        {
            throw new DalDoesNotExistException($"An Engineer with id={item.Id} does not exist.\n");
        }
    }

    /// <summary>
    /// delete the engineer with the given id if such exists, if not throws exception
    /// </summary>
    /// <param name="id">the id of the engineer to delete</param>
    /// <exception cref="DalDoesNotExistException"></exception>    
    public void Delete(int id)
    {
        //if such index exists we will replace that Engineer with a new, non active engineer
        //and if this index doesn't exist, meaning there isn't Engineer with such id, we will throw an exception
        int index=DataSource.Engineers.FindIndex((en) => en.Id == id);
        if (index != -1) 
        {
            //would have done here
            //Engineer non_active_engineer = DataSource.Engineers[index] with {Active=false };
            //DataSource.Engineers.Add(non_active_engineer);

            //but instead we do here only
            DataSource.Engineers.RemoveAt(index);
        }
        else throw new DalDoesNotExistException($"there isn't an Engineer with Id={id}.\n"); 
    }
}

