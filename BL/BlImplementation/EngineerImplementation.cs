namespace BlImplementation;

using System.Data.Common;
using BlApi;


internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    public int Create(BO.Engineer boEngineer)
    {
        DO.Engineer doEngineer = new DO.Engineer(
            Id:boEngineer.Id,
            Email:boEngineer.Email,
            Cost:boEngineer.Cost,
            Name:boEngineer.Name,
            Level:(DO.EngineerExperience)(int)boEngineer.Level,
            Active:true
            );
        try
        {
            int idEngineer = _dal.Engineer.Create(doEngineer);
            return idEngineer;
        }
        catch (DO.DalAlreadyExistsException ex) 
        {
            throw new BO.BlAlreadyExistsException($"Engineer with ID={boEngineer.Id} already exist", ex);
        }
    }

    public void Delete(int id)
    {
        try 
        {  
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        { 
         throw 
        }
        
    }

    public BO.Engineer? Read(int id)
    {
        DO.Engineer? doEngineer = _dal.Engineer.Read(id);

        if(doEngineer == null)
            throw new BO.BlAlreadyExistsException($"Engineer with ID={id} doesn't exist");

        return new BO.Engineer() {
              Id= doEngineer.Id,
            Email= doEngineer.Email,
            Cost= doEngineer.Cost,
            Name= doEngineer.Name,
            Level= (BO.EngineerExperience)(int)doEngineer.Level,
            };

    }

    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool> filter = null)
    {
        return (from DO.Engineer doEngineer in _dal.Engineer.ReadAll()
                select new BO.Engineer
                {
                    Id = doEngineer.Id,
                    Name = doEngineer.Name
                });
    }

    public void Update(BO.Engineer itemBoEngineer)
    {
        
    }
}
