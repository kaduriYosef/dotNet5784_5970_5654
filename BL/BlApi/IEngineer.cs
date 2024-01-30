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
    public IEnumerable<BO.Engineer> ReadAll();
    public void Update(BO.Engineer boEngineer);
    public void Delete(int id);

    public BO.EngineerInTask(int engineerId,int taskId); //to implement

}
