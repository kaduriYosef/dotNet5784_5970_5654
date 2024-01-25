using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;

namespace Dal;

sealed internal class DalXml: IDal
{
    public static IDal Instance { get; } = new DalXml();
    private DalXml() { }
    public ITask Task => new TaskImplementation();
    public IDependency Dependency => new DependencyImplementation();
    public IEngineer Engineer => new EngineerImplementation();
}
