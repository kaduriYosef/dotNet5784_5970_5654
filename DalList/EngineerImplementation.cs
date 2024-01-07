
namespace Dal;
using DalApi;
using DO;
using System.Reflection.Metadata.Ecma335;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        bool isNew = !DataSource.Engineers.Any(en => en.Id == item.Id);
        if (isNew) DataSource.Engineers.Add(item);
        return isNew ? item.Id : throw new Exception($"Engineer with Id={item.Id} already exist.\n");
    }

    public Engineer? Read(int id)
    {
       
        return DataSource.Engineers.FirstOrDefault(result => result.Id == id);
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);
    }


    public void Update(Engineer item)
    {
        int index = DataSource.Engineers.FindIndex(e => e.Id == item.Id);

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

        throw new NotImplementedException();
    }
}

