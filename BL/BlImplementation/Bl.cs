using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlImplementation;
using BlApi;

/// <summary>
/// Provides an implementation for the IBl interface, serving as the entry point for the business logic layer.
/// </summary>
internal class Bl : IBl
{
    /// <summary>
    /// Provides access to the engineer-related functionalities.
    /// </summary>
    public IEngineer Engineer => new EngineerImplementation();

    /// <summary>
    /// Provides access to the task-related functionalities.
    /// </summary>
    public ITask Task => new TaskImplementation(this);

    /// <summary>
    /// Provides access to the milestone-related functionalities.
    /// </summary>
    public IMilestone Milestone => new MilestoneImplementation();
    private static DateTime s_Clock = DateTime.Now;
    public DateTime Clock { get { return s_Clock; } private set { s_Clock = value; } }


    public void AddSeconds(int sec)
    {
        s_Clock=s_Clock.AddSeconds(sec);
    }

    public void AddMinutes(int min)
    {
        s_Clock= s_Clock.AddMinutes(min);
    }
    public void AddDay()
    {
        s_Clock = s_Clock.AddDays(1); // Adds one day to the current DateTime stored in s_Clock
    }

    public void AddHour()
    {
        s_Clock = s_Clock.AddHours(1); // Adds one hour to the current DateTime stored in s_Clock
    }

    public void AddYear()
    {
        s_Clock = s_Clock.AddYears(1); // Adds one year to the current DateTime stored in s_Clock
    }

    public void ResetClock()
    {
        s_Clock = DateTime.Now; // Resets s_Clock to the current date and time
    }


    public void InitializeDB() => DalTest.Initialization.Do();

    public void ResetDB() => DalTest.Initialization.Reset();


}