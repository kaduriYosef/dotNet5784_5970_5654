
using BlApi;
using BO;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    public int Create(Milestone item)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public double PrecantegeComplete(int id)
    {
        throw new NotImplementedException();
    }

    public Milestone? Read(int id)
    {
        throw new NotImplementedException();
    }

    public Milestone? Read(Func<Milestone, bool> filter)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Milestone?> ReadAll(Func<Milestone, bool>? filter = null)
    {
        throw new NotImplementedException();
    }

    public void Update(Milestone item)
    {
        throw new NotImplementedException();
    }
}
