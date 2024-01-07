
namespace Dal;
using DalApi;
using DO;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        throw new NotImplementedException();
    }


    public Dependency? Read(int id)
    {
        throw new NotImplementedException();
    }

    public List<Dependency> ReadAll()
    {
        throw new NotImplementedException();
    }

    public void Update(Dependency item)
    {
        throw new NotImplementedException();
    }
    public void Delete(int id)
    {
        //if such index exists we will remove this Dependency
        //and if this index doesn't exist, meaning there isn't Dependency with such id, we will throw an exception
        int index = DataSource.Dependencies.FindIndex((en) => en.Id == id);
        if (index != -1)
            DataSource.Dependencies.RemoveAt(index);
        else
            throw new Exception($"there isn't an Dependency with Id={id}.\n");
    }
}
