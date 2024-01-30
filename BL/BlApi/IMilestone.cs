﻿

namespace BlApi;

public interface IMilestone
{
    public IEnumerable<BO.Milestone> ReadAll(Func<BO.Milestone,bool> filter=null);

    public BO.Milestone Read(int id);

    public int Create(BO.Milestone itemBoMilestone);

    public void Update(BO.Milestone itemBoMilestone);

    public void Delete(int id);

    public double PrecentegeComplete(int id);
}






