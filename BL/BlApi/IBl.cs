using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;

public interface IBl
{
    public DateTime? StartDate { get; }
    public IEngineer Engineer { get; }

    public ITask Task { get; }

    //public IMilestone Milestone { get; }
    
    #region Clock
    public DateTime Clock { get; }
    public void SaveClock(DateTime c);
    public void AddSeconds(int sec);
    public void AddMinutes(int min);
    public void AddHour();
    public void AddDay();
    public void AddYear();
    public void ResetClock();

    #endregion 

    public void InitializeDB();

    public void ResetDB();


}
