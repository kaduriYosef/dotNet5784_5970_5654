

namespace BlApi;

public interface IMilestone :ICrud<BO.Milestone>
{
    //public IEnumerable<BO.Milestone> ReadAll(Func<BO.Milestone,bool> filter=null);

    //public BO.Milestone Read(int id);

    //public int Create(BO.Milestone itemBoMilestone);

    //public void Update(BO.Milestone itemBoMilestone);

    //public void Delete(int id);

    public double PrecantegeComplete(int id);
}






