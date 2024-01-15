
namespace Dal;
using DalApi;
using DO;
using System.Reflection.Metadata.Ecma335;

internal class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        bool isNew = !DataSource.Engineers.Any(en => en.Id == item.Id);
        if (isNew) DataSource.Engineers.Add(item);
        return isNew ? item.Id : throw new Exception($"Engineer with Id={item.Id} already exist.\n");
    }

    public Engineer? Read(int id)
    {
       
        return DataSource.Engineers.FirstOrDefault(eng => eng.Id == id);
    }

    public Engineer? Read(Func<Engineer, bool> filter) // stage 2
    {
        if (filter == null) return null;
        return DataSource.Engineers.FirstOrDefault(eng => filter(eng));
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null) // stage 2
    {
        if (filter == null)
            return DataSource.Engineers.Select(item => item);
        else
            return DataSource.Engineers.Where(filter);
    }

    
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
            throw new Exception($"An Engineer with id={item.Id} does not exist.\n");
        }
    }    
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
        else throw new Exception($"there isn't an Engineer with Id={id}.\n"); 
    }
}

