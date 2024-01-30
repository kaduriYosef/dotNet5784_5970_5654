using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IEngineer
{
    public int Create(BO.Engineer itemBoEngineer);
    public BO.Engineer? Read(int id);
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool> filter = null);
    public void Update(BO.Engineer itemBoEngineer);
    public void Delete(int id);
    
}
