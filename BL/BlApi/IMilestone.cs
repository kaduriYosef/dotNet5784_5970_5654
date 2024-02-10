namespace BlApi;

/// <summary>
/// Defines the interface for milestone management, extending basic CRUD operations with additional functionality specific to milestones.
/// </summary>
public interface IMilestone : ICrud<BO.Milestone>
{
    // These commented methods are already defined in the ICrud interface and thus do not need to be redefined here.

    //public IEnumerable<BO.Milestone> ReadAll(Func<BO.Milestone,bool> filter=null);
    //public BO.Milestone Read(int id);
    //public int Create(BO.Milestone itemBoMilestone);
    //public void Update(BO.Milestone itemBoMilestone);
    //public void Delete(int id);

    /// <summary>
    /// Calculates and returns the percentage of completion for a specific milestone.
    /// </summary>
    /// <param name="id">The ID of the milestone for which the completion percentage is to be calculated.</param>
    /// <returns>The completion percentage of the milestone.</returns>
    public double PercentageComplete(int id);
}
