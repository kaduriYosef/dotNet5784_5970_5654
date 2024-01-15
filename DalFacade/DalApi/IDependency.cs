
using DO;

namespace DalApi;

public interface IDependency : ICrud<Dependency>
{
  
    public bool DoesExist(int dependent_id, int dependsOn_id);
}
