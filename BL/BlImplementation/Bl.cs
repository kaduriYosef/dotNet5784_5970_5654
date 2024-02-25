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
    public ITask Task => new TaskImplementation();

    /// <summary>
    /// Provides access to the milestone-related functionalities.
    /// </summary>
    public IMilestone Milestone => new MilestoneImplementation();

    public void InitializeDB() => DalTest.Initialization.Do();

   // public void ResetDB() => DalTest.Initialization.Reset();
}
