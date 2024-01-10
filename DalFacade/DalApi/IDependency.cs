
using DO;

namespace DalApi;

public interface IDependency
{
    //Creates new entity object in DAL
    int Create(Dependency item); 
    
    //Reads entity object by its ID 
    Dependency? Read(int id); 
   
    //stage 1 only, Reads all entity objects
    List<Dependency> ReadAll(); 
    
    //Updates entity object
    void Update(Dependency item); 
    
    //Deletes an object by its Id
    void Delete(int id); 
    
    //show whether there exist such a dependency
    public bool DoesExist(int dependent_id, int dependsOn_id); 
}
