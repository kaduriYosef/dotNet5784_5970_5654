using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

internal interface IEngineer
{
    public int Create(BO.Engineer boEndineer);
    public BO.Engineer? Read(int id);
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool> filter = null);
    public void Update(BO.Engineer item);
    public void Delete(int id);

    public BO.EngineerInTask(int engineerId,int taskId); //to implement

}
