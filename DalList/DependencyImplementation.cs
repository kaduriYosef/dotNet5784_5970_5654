
namespace Dal;
using DalApi;
using DO;

public class DependencyImplementation : IDependency
{
    public int Create(Dependency item)
    {
        int id = DataSource.Config.NextDependencyId;
        Dependency new_item = item with { Id = id };
        DataSource.Dependencies.Add(new_item);
        return id;

    }


    public Dependency? Read(int id)
    {
        return DataSource.Dependencies.FirstOrDefault(dep => dep.Id == id);
    }

    public List<Dependency> ReadAll()
    {
        return new List<Dependency>(DataSource.Dependencies);
    }

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
            throw new Exception($"An Dependency with id={item.Id} does not exist.\n");
        }
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
    public bool DoesExist(int dependent_id,int dependsOn_id)
    {
        return DataSource.Dependencies.Any(dep=>dep.DependentTask == dependent_id && dep.DependsOnTask==dep.DependsOnTask);
    }
}
