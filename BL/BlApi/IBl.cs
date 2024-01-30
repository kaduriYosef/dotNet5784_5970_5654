using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IBl
{
    public IEngineer Engineer { get; }
    public IDependency Dependency { get; }
    public Itask Task { get; }  

}
